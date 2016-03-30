using UnityEngine;
using System.Collections;

public class KillVolume : MonoBehaviour {

    GameManager gm;

	// Use this for initialization
	void Start () {
        gm = GameObject.Find("Connect_Test").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(PhotonNetwork.isMasterClient)
        {
            if(other.gameObject.GetComponent<Player>())
            {
                int id = other.gameObject.GetComponent<PhotonView>().ownerId;

                gm.SelfKill(id);
            }
        }
    }

}
