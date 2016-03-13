using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerManager : BaseClass
{
    public GameManager gm;

    public Text currentTeam;
    public Text playCountDown;
    public Text readyUpCheck;
    public Text playerCount;
    public GameObject readyNotification;
    
    bool inGame;
    bool ready;

    //public int team;
    void Awake()
    {
        inGame = false;
    }

    void Update()
    {
        //Debug.Log(PhotonNetwork.player.ID);

        currentTeam.text = "Team " + gm.myTeam;
        playerCount.text = "Players: " + gm.NumPlayers();
        playCountDown.text = "Starting in: " + gm.countDownLength.ToString("F1");

        if (!inGame)
        {
            if (Input.GetButtonDown("Ready"))
            {
                if(!ready)
                {
                    ready = true;
                    readyUpCheck.text = "Ready!";
                    gm.ReadyUp();
                }
                /*if (ready)
                {
                    readyUpCheck.text = "Not Ready";
                }
                else
                {
                    readyUpCheck.text = "Ready!";
                }

                ready = !ready;*/
            }
        }
    }


    void OnJoinedRoom()
    {
        if (PlayerPrefs.GetString("CurrentCharacter") == "Private Puce")
        {
            PhotonNetwork.Instantiate("PlayerPuce", Vector3.zero, Quaternion.identity, 0);
        }
        else if (PlayerPrefs.GetString("CurrentCharacter") == "Sargent Spice")
        {
            PhotonNetwork.Instantiate("PlayerSpice", Vector3.zero, Quaternion.identity, 0);
        }
        else if (PlayerPrefs.GetString("CurrentCharacter") == "Admiral Amber")
        {
            PhotonNetwork.Instantiate("PlayerAmber", Vector3.zero, Quaternion.identity, 0);
        }
        else
        {
            PhotonNetwork.Instantiate("PlayerPuce", Vector3.zero, Quaternion.identity, 0);
        }
    }
}
