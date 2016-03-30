using UnityEngine;
using System.Collections;

public class JumpModule : MonoBehaviour {
    CharacterController Controller;
    float ySpeed;
    public float jumpSpeed;
    public float gravity;

    // Use this for initialization
    void Start () {
        Controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        

       

        if (Controller.isGrounded)
        {
            ySpeed = 0f;

            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("Time to Jump");
                ySpeed = jumpSpeed;
            }

        }

        ySpeed -= gravity * Time.deltaTime;

        GetComponent<Player>().SetMoveY(ySpeed);
        
    }
}
