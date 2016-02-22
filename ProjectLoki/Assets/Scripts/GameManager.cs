using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour
{
    void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);  
    }       
}
