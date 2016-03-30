using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : BaseClass
{
    public GameObject[] spawnPoints;

    //info about me
    public GameObject me;

    bool ready;
    bool inGame;

    Dictionary<int, PlayerInfo> playerDictionary = new Dictionary<int, PlayerInfo>();

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

    //UI stuff
    public GameObject mainCanvas;

    public Text currentTeam;
    public Text currentPlayer;
    public Text playCountDown;
    public Text readyUpCheck;
    public Text playerCount;
    public GameObject readyNotification;

    public GameObject DeathImage;
    public GameObject EndImage;
    public Text endText;

    public GameObject hitImage;

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
        if (inGame)
        {
            currentTeam.text = "Team " + playerDictionary[PhotonNetwork.player.ID].team;
        }
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

                readyNotification.SetActive(false);

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

        //updatescoreboard
        team1kills.text = "Kills: " + team1Points;
        team2kills.text = "Kills: " + team2Points;

        string team1playerinfo = "";
        string team2playerinfo = "";

        foreach(int pl in playerDictionary.Keys)
        {
            if(playerDictionary[pl].team == 1)
            {
                team1playerinfo += "Player " + pl + "    " + playerDictionary[pl].kills + "    " + playerDictionary[pl].deaths + "\n";
            }

            if (playerDictionary[pl].team == 2)
            {
                team2playerinfo += "Player " + pl + "    " + playerDictionary[pl].kills + "    " + playerDictionary[pl].deaths + "\n";
            }
        }

        team1Players.text = team1playerinfo;
        team2Players.text = team2playerinfo;

        if(inGame)
        {
            DecreaseRespanws();
        }

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

        //this is to test damage indicators
        if(Input.GetButtonDown("TestHitFront"))
        {
            Vector3 tester = new Vector3(0, 0, -1);

            ShowDamageMarkers(tester);
        }

        //this is to test damage indicators
        if (Input.GetButtonDown("TestHitBack"))
        {
            Vector3 tester = new Vector3(0, 0, 1);

            ShowDamageMarkers(tester);
        }

        //this is to test damage indicators
        if (Input.GetButtonDown("TestHitLeft"))
        {
            Vector3 tester = new Vector3(1, 0, 0);

            ShowDamageMarkers(tester);
        }

        //this is to test damage indicators
        if (Input.GetButtonDown("TestHitRight"))
        {
            Vector3 tester = new Vector3(-1, 0, 0);

            ShowDamageMarkers(tester);
        }

        //Debug.Log(me.transform.eulerAngles.y);
    }

    public void ShowDamageMarkers(Vector3 hit)
    {
        GameObject tempInd = (GameObject)Instantiate(hitImage, Vector3.zero, Quaternion.identity);
        tempInd.transform.SetParent(mainCanvas.transform);
        tempInd.transform.localPosition = Vector3.zero;

        float opp = 0 - hit.z;
        float adj = 0 - hit.x;

        float angle = Mathf.Atan2(opp, adj);
        angle *= Mathf.Rad2Deg;
        angle -= 90f;

        angle += me.transform.eulerAngles.y;

        Debug.Log(angle);

        tempInd.transform.localEulerAngles = new Vector3(0f, 0f, angle);
    }

    //its expensive to change all the text so we only want to do this when we need to.
    //ideally we will only change parts but for now we'll just refresh it all when they call the scoreboard
    void UpdateUIText()
    {

    }

    //This decreases the respawns for all dead players.  Note that this is just display information for clients where
    //this server will actually do something if a cound hits zero
    void DecreaseRespanws()
    {
        List<int> tempPlayers = new List<int> (playerDictionary.Keys);

        foreach(int current in tempPlayers)
        {
            if(playerDictionary[current].dead)
            {
                if(playerDictionary[current].currentRespawnTime <= 0f)
                {
                    if(PhotonNetwork.isMasterClient)
                    {
                        playerDictionary[current].dead = false;
                        playerDictionary[current].currentRespawnTime = 0f;

                        GetComponent<PhotonView>().RPC("RequestSpawn", PhotonTargets.All, current, playerDictionary[current].team);
                    }
                } else
                {
                    playerDictionary[current].currentRespawnTime -= Time.deltaTime;
                }
            }
        }
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
                    GetComponent<PhotonView>().RPC("RequestSpawn", PhotonTargets.All, player.ID, 1);
                } else
                {
                    //playerTeams.Add(player.ID, 2);
                    GetComponent<PhotonView>().RPC("GiveTeam", PhotonTargets.AllBuffered, player.ID, 2);
                    GetComponent<PhotonView>().RPC("RequestSpawn", PhotonTargets.All, player.ID, 2);
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
        playerDictionary[killerId].kills += 1;
        playerDictionary[victimId].deaths += 1;

        if(playerDictionary[killerId].team == 1)
        {
            team1Points += 1;
        } else
        {
            team2Points += 1;
        }

        playerDictionary[victimId].dead = true;
        playerDictionary[victimId].currentRespawnTime = respawnTime;

        if (victimId == PhotonNetwork.player.ID)
        {
            DeathImage.SetActive(true);
            me.GetComponent<Player>().Die();
        }

        if (PhotonNetwork.isMasterClient) //the master clients check to see if the game is over
        {
            if(team1Points >= pointsToWin)
            {
                //endgame
                GetComponent<PhotonView>().RPC("EndGame", PhotonTargets.All, 1);
            } else if(team2Points >= pointsToWin)
            {
                //endgame
                GetComponent<PhotonView>().RPC("EndGame", PhotonTargets.All, 2);
            }
        }
    }

    [PunRPC]
    void EndGame(int winningTeam)
    {
        EndImage.SetActive(true);
        if(playerDictionary[PhotonNetwork.player.ID].team == winningTeam)
        {
            endText.text = "YOU WIN!";
        } else
        {
            endText.text = "YOU LOSE!";
        }
    }

    //Lets everyone know a client died. the master will then schedule a respanw for them.
    //This is just for general death. Use kill confirmed for deaths with points
    [PunRPC]
    void IDied(int id)
    {
        playerDictionary[id].dead = true;
        playerDictionary[id].currentRespawnTime = respawnTime;
    }

    public void SelfKill(int id)
    {
        GetComponent<PhotonView>().RPC("Suicide", PhotonTargets.All, id);
    }

    //Use this for self kills
    [PunRPC]
    void Suicide(int id)
    {
        playerDictionary[id].deaths += 1;

        if (playerDictionary[id].team == 1)
        {
            team1Points -= 1;
        }
        else
        {
            team2Points -= 1;
        }

        playerDictionary[id].dead = true;
        playerDictionary[id].currentRespawnTime = respawnTime;

        if (id == PhotonNetwork.player.ID)
        {
            DeathImage.SetActive(true);
            me.GetComponent<Player>().Die();
        }
    }

    //this is run on the master only. given the team it will run an algorithm to determine where the
    //player should spawn.  right now it just checks the team and puts it in the proper side.
    [PunRPC]
    void RequestSpawn(int id, int team)
    {
        if(PhotonNetwork.isMasterClient)
        {

            int pick = Random.Range(0, spawnPoints.Length);

            Vector3 pos = spawnPoints[pick].transform.position;
            float rot = spawnPoints[pick].transform.eulerAngles.y;

            if(team == 1)
            {
                GetComponent<PhotonView>().RPC("RecieveSpawn", PhotonTargets.All, id, pos, rot);
            } else
            {
                GetComponent<PhotonView>().RPC("RecieveSpawn", PhotonTargets.All, id, pos, rot);
            }
        }
    }

    //tells the client where to spawn at
    [PunRPC]
    void RecieveSpawn(int id, Vector3 spawn, float rot)
    {
        if(id == PhotonNetwork.player.ID)
        {
            // me.transform.position = spawn;
            DeathImage.SetActive(false);
            me.GetComponent<Player>().Respawn(spawn, rot);
        }
    }

    //tells a client what team its on
    [PunRPC]
    public void GiveTeam(int id, int team)
    {
        playerDictionary[id].team = team;
    }

    //begins the game
    [PunRPC]
    public void BeginGame()
    {
        Debug.Log("We are now trying to kill each other");
        inGame = true;
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

    //players call this when they join, creating their info
    [PunRPC]
    void CreateMyInfo(int id)
    {
        if(PhotonNetwork.isMasterClient)
        {
            PlayerInfo temp = new PlayerInfo();
            temp.playerNumber = id;

            temp.kills = 0;
            temp.deaths = 0;
            
            //if the game is in progress assign to the imbalanced team and give a respawn, else just put them in the lobby
            if (inGame)
            {

            } else
            {
                temp.team = 0;
                temp.dead = false;
                temp.currentRespawnTime = 0f;
            }

            playerDictionary[id] = temp;
        }
    }

    //this sends a single players info (does not update on master to prevent concurrent editing error)
    [PunRPC]
    void SendPlayerInfo(int playerNumber, int playerTeam, int playerKills, int playerDeaths, bool playerDead, float playerRespawn)
    {
        if(!PhotonNetwork.isMasterClient)
        {
            PlayerInfo temp = new PlayerInfo();
            temp.playerNumber = playerNumber;
            temp.team = playerTeam;
            temp.kills = playerKills;
            temp.deaths = playerDeaths;
            temp.dead = playerDead;
            temp.currentRespawnTime = playerRespawn;

            playerDictionary[playerNumber] = temp;
        }
    }

    //Request info on all players, this is what a joining player would call
    [PunRPC]
    void RequestAllPlayerInfo()
    {
        SendAllPlayerInfo();
    }

    //the master updates the info for every player in the game (think of it as an all-synch)
    void SendAllPlayerInfo()
    {
        if(PhotonNetwork.isMasterClient)
        {
            foreach(int current in playerDictionary.Keys)
            {
                PlayerInfo temp = playerDictionary[current];
                GetComponent<PhotonView>().RPC("SendPlayerInfo", PhotonTargets.All, current, temp.team, temp.kills, temp.deaths, temp.dead, temp.currentRespawnTime);
            }
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

       GetComponent<PhotonView>().RPC("CreateMyInfo", PhotonTargets.MasterClient, PhotonNetwork.player.ID);

        GetComponent<PhotonView>().RPC("RequestAllPlayerInfo", PhotonTargets.MasterClient);
    }
}
