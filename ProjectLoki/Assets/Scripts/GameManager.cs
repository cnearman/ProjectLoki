using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : BaseClass
{
    //info about me
    public GameObject me;
    public int myTeam;

    bool ready;
    bool inGame;

    //team list
    Dictionary<int, int> playerTeams = new Dictionary<int, int>();

    //respawn stuff
    Dictionary<int, float> respawns = new Dictionary<int, float>();
    List<int> decRespawn = new List<int>();
    List<int> clearRespawn = new List<int>();

    public Vector3 spawnT1;
    public Vector3 spawnT2;

    public float respawnTime;

    //pregame stuff
    bool countingDown;
    public float countDownLength;
    
    public int numberOfPlayers;
    public int numberOfReadys;

    //DM mode stuff
    public int pointsToWin;
    public int team1Points;
    public int team2Points;

    Dictionary<int, int> playersKills = new Dictionary<int, int>();
    Dictionary<int, int> playersDeaths = new Dictionary<int, int>();

    //UI stuff
    public Text currentTeam;
    public Text currentPlayer;
    public Text playCountDown;
    public Text readyUpCheck;
    public Text playerCount;
    public GameObject readyNotification;

    //scoreboard
    public GameObject scoreboard;
    public Text team1kills;
    public Text team2kills;

    public Text team1Players;
    public Text team2Players;

    

    void Update()
    {
        numberOfPlayers = PhotonNetwork.playerList.Length; //probably don't want to do this every update

        //update the ui with game info
        currentTeam.text = "Team " + myTeam;
        currentPlayer.text = "Player " + PhotonNetwork.player.ID;
        playerCount.text = "Players: " + numberOfPlayers;
        playCountDown.text = "Starting in: " + countDownLength.ToString("F1");

        //if not in game (so in warmup) let players ready up
        if (!inGame)
        {
            if (Input.GetButtonDown("Ready"))
            {
                if (!ready)
                {
                    ready = true;
                    readyUpCheck.text = "Ready!";
                    ReadyUp();
                }
            }
        }

        //countdown to game starts. for clients this is just info. for the master it tells it when to start
        if (countingDown)
        {
            if(countDownLength <= 0)
            {
                countingDown = false;

                if(PhotonNetwork.isMasterClient)
                {
                    GetComponent<PhotonView>().RPC("BeginGame", PhotonTargets.AllBuffered);
                    PhotonPlayer[] players = PhotonNetwork.playerList;

                    foreach (PhotonPlayer player in players)
                    {
                        
                        GetComponent<PhotonView>().RPC("SetNoMove", PhotonTargets.All, player.ID, true);
                    }
                }

            } else
            {
                countDownLength -= Time.deltaTime;
            }
        }

        ReduceRespawns();

        //updatescoreboard
        team1kills.text = "Kills: " + team1Points;
        team2kills.text = "Kills: " + team2Points;

        string team1playerinfo = "";
        string team2playerinfo = "";

        foreach(int pl in playerTeams.Keys)
        {
            if(playerTeams[pl] == 1)
            {
                team1playerinfo += "Player " + pl + "    " + playersKills[pl] + "    " + playersDeaths[pl] + "\n";
            }

            if (playerTeams[pl] == 2)
            {
                team2playerinfo += "Player " + pl + "    " + playersKills[pl] + "    " + playersDeaths[pl] + "\n";
            }
        }

        team1Players.text = team1playerinfo;
        team2Players.text = team2playerinfo;

        //open/close scoreboard
        if(Input.GetButtonDown("Score"))
        {
            scoreboard.SetActive(true);
        }

        if (Input.GetButtonUp("Score"))
        {
            scoreboard.SetActive(false);
        }

        //this is to test if respawn is working
        if (Input.GetButtonDown("TestDeath"))
        {
            GetComponent<PhotonView>().RPC("IDied", PhotonTargets.AllBuffered, PhotonNetwork.player.ID);
        }

        //this is to test if killing is working
        if (Input.GetButtonDown("TestKill"))
        {
            GetComponent<PhotonView>().RPC("KillConfirmed", PhotonTargets.AllBuffered, PhotonNetwork.player.ID, PhotonNetwork.player.ID);
        }
    }

    //tick down the respawns and if master respawn them
    public void ReduceRespawns()
    {
        foreach(int pl in respawns.Keys)
        {
            decRespawn.Add(pl);
        }

        foreach(int dc in decRespawn)
        {
            respawns[dc] -= Time.deltaTime;
            if (respawns[dc] <= 0f)
            {
                clearRespawn.Add(dc);
            }
        }

        foreach(int rm in clearRespawn)
        {
            respawns.Remove(rm);
            if (PhotonNetwork.isMasterClient)
            {
                GetComponent<PhotonView>().RPC("RequestSpawn", PhotonTargets.All, playerTeams[rm], rm);
            }
        }

        decRespawn.Clear();
        clearRespawn.Clear();
    }

    //Lets eveyone know that this client is ready
    public void ReadyUp()
    {
        numberOfReadys += 1;
        GetComponent<PhotonView>().RPC("UpdateReadys", PhotonTargets.AllBuffered, numberOfReadys);
    }

    //Checks if all clients are ready. If they are, the master tells all clients that the game is starting
    void CheckIfAllReady()
    {
        if(PhotonNetwork.isMasterClient)
        {
            if (numberOfReadys >= numberOfPlayers)
            {
                GetComponent<PhotonView>().RPC("StartGame", PhotonTargets.AllBuffered);
            }
        }
    }

    //starts the countdown to gamestart. the master will assign everyone teams and will
    //move them to their spawns
    void ActivateCountDown()
    {
        countingDown = true;
        if(PhotonNetwork.isMasterClient)
        {
            PhotonPlayer[] players = PhotonNetwork.playerList;

            bool team1 = true;
            foreach(PhotonPlayer player in players)
            {
                if(team1)
                {
                    //playerTeams.Add(player.ID, 1);
                    GetComponent<PhotonView>().RPC("GiveTeam", PhotonTargets.AllBuffered, player.ID, 1);
                    GetComponent<PhotonView>().RPC("RequestSpawn", PhotonTargets.All, 1, player.ID);
                } else
                {
                    //playerTeams.Add(player.ID, 2);
                    GetComponent<PhotonView>().RPC("GiveTeam", PhotonTargets.AllBuffered, player.ID, 2);
                    GetComponent<PhotonView>().RPC("RequestSpawn", PhotonTargets.All, 2, player.ID);
                }

                GetComponent<PhotonView>().RPC("SetNoMove", PhotonTargets.All, player.ID, false);

                team1 = !team1;
            }
        }
    }

    //call this when someone kills someone. this will probably be called from a different script (thats why its public)
    [PunRPC]
    public void KillConfirmed(int killerId, int victimId)
    {
        playersKills[killerId] += 1;
        playersDeaths[victimId] += 1;

        if(playerTeams[killerId] == 1)
        {
            team1Points += 1;
        } else
        {
            team2Points += 1;
        }

        if (PhotonNetwork.isMasterClient) //this may be added to clients later MOBA style
        {
            respawns.Add(victimId, respawnTime);
        }

        if (PhotonNetwork.isMasterClient) //the master clients check to see if the game is over
        {
            if(team1Points >= pointsToWin)
            {
                //endgame
            } else if(team2Points >= pointsToWin)
            {
                //endgame
            }
        }
    }

    //Lets everyone know a client died. the master will then schedule a respanw for them.
    //This is just for general death. Use kill confirmed for deaths with points
    [PunRPC]
    void IDied(int id)
    {
        if (PhotonNetwork.isMasterClient) //this may be added to clients later MOBA style
        {
            respawns.Add(id, respawnTime);
        }
    }

    //this is run on the master only. given the team it will run an algorithm to determine where the
    //player should spawn.  right now it just checks the team and puts it in the proper side.
    [PunRPC]
    void RequestSpawn(int team, int id)
    {
        if(PhotonNetwork.isMasterClient)
        {
            if(team == 1)
            {
                GetComponent<PhotonView>().RPC("RecieveSpawn", PhotonTargets.All, spawnT1, id);
            } else
            {
                GetComponent<PhotonView>().RPC("RecieveSpawn", PhotonTargets.All, spawnT2, id);
            }
        }
    }

    //tells the client where to spawn at
    [PunRPC]
    void RecieveSpawn(Vector3 spawn, int id)
    {
        if(id == PhotonNetwork.player.ID)
        {
            me.transform.position = spawn;
        }
    }

    //tells a client what team its on
    [PunRPC]
    public void GiveTeam(int id, int team)
    {
        if(id == PhotonNetwork.player.ID)
        {
            Debug.Log("got team");
            myTeam = team;
        }

        playerTeams.Add(id, team);
        playersKills.Add(id, 0);
        playersDeaths.Add(id, 0);
    }

    //begins the game
    [PunRPC]
    public void BeginGame()
    {
        Debug.Log("We are now trying to kill each other");
    }

    //start countdown to game start
    [PunRPC]
    public void StartGame()
    {
        Debug.Log("Starting Game");
        ActivateCountDown();
    }

    //update the number of ready players across clients
    [PunRPC]
    public void UpdateReadys(int readys)
    {
        Debug.Log("recieved ready");
        numberOfReadys = readys;
        CheckIfAllReady();
    }

    //sets the move state of a client
    [PunRPC]
    void SetNoMove(int id, bool canMove)
    {
        if(PhotonNetwork.player.ID == id)
        {
            me.GetComponent<Player>().noMove = !canMove;
        }
    }

    //creates a character for the player based on player selection
    void OnJoinedRoom()
    {
        if(PlayerPrefs.GetString("CurrentCharacter") == "")
        {
            me = PhotonNetwork.Instantiate("PlayerPuce", Vector3.zero, Quaternion.identity, 0);
        } else
        {
            me = PhotonNetwork.Instantiate(PlayerPrefs.GetString("CurrentCharacter"), Vector3.zero, Quaternion.identity, 0);
        }
    }
}
