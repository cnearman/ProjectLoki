using UnityEngine;
using System.Collections;

public class GameManager : BaseClass
{
    public PlayerManager pm;

    public int myTeam;

    bool inGame;

    bool countingDown;
    public float countDownLength;

    public int numberOfPlayers;
    public int numberOfReadys;

    void Update()
    {
        numberOfPlayers = PhotonNetwork.playerList.Length; //probably don't want to do this every update

        if(countingDown)
        {
            if(countDownLength <= 0)
            {
                countingDown = false;

                if(PhotonNetwork.isMasterClient)
                {
                    GetComponent<PhotonView>().RPC("BeginGame", PhotonTargets.AllBuffered);
                }

            } else
            {
                countDownLength -= Time.deltaTime;
            }
        }
    }

    public int NumPlayers()
    {
        return numberOfPlayers;
    }

    public void ReadyUp()
    {
        numberOfReadys += 1;
        GetComponent<PhotonView>().RPC("UpdateReadys", PhotonTargets.AllBuffered, numberOfReadys);
        
    }

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
                    GetComponent<PhotonView>().RPC("GiveTeam", PhotonTargets.AllBuffered, player.ID, 1);
                } else
                {
                    GetComponent<PhotonView>().RPC("GiveTeam", PhotonTargets.AllBuffered, player.ID, 2);
                }
                team1 = !team1;
            }
        }
    }

    [PunRPC]
    public void GiveTeam(int id, int team)
    {
        if(id == PhotonNetwork.player.ID)
        {
            Debug.Log("got team");
            myTeam = team;
        }
    }

    [PunRPC]
    public void BeginGame()
    {
        Debug.Log("We are now trying to kill each other");
    }

    [PunRPC]
    public void StartGame()
    {
        Debug.Log("Starting Game");
        ActivateCountDown();
    }

    [PunRPC]
    public void UpdateReadys(int readys)
    {
        Debug.Log("recieved ready");
        numberOfReadys = readys;
        CheckIfAllReady();
    }
}
