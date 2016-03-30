using UnityEngine;
using System.Collections;
using System.Linq;
using ProjectLoki.Weapons;

public class Player : BaseClass {

    CharacterController Controller;
    PhotonView m_PhotonView;
    AbilitiesController Abilities;
    PhotonTransformView m_PhotonTransformView;
    Vector3 moveDirection;
    

    public float speed;
    public float jumpSpeed;

    public float MotionInX { get; set; }
    public float MotionInY { get; set; }
    public float RotationLateral { get; set; }
    public float RotationLongitudinal { get; set; }

    public GameObject CameraContainer;

    private Vector3 currentPosition;
    private Quaternion currentRotation;

    //these are different movement states. i put them here but there may be a better place
    public bool noMove; //you can look but you cant move (game countdown)
    public bool dead; //you dead. no move, collider and such inactive

    //called on death
    public void Die()
    {
        if (m_PhotonView.isMine)
        {
            m_PhotonView.RPC("KillThisPlayer", PhotonTargets.All);
        }
    }

    //called on respawn
    public void Respawn(Vector3 spawnPoint, float rot)
    {
        if (m_PhotonView.isMine)
        {
            m_PhotonView.RPC("SpawnThisPlayer", PhotonTargets.All, spawnPoint, rot);
        }
    }


    //disables all things that need to be disabled across clients on death
    [PunRPC]
    void KillThisPlayer()
    {
        dead = true;

        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
    }

    //enables and resets values across clients on respawn
    [PunRPC]
    void SpawnThisPlayer(Vector3 spawnPoint, float rot)
    {
        transform.position = spawnPoint;

        dead = false;

        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<CharacterController>().enabled = true;

        RotationLateral = rot;
    }


    // Use this for initialization
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        m_PhotonView = GetComponent<PhotonView>();
        m_PhotonTransformView = GetComponent<PhotonTransformView>();
        Abilities = GetComponent<AbilitiesController>();

        m_PhotonView = GetComponent<PhotonView>();
        if (m_PhotonView.isMine)
        {
            CameraContainer.GetComponent<Camera>().enabled = true;
            m_PhotonTransformView.SetSynchronizedValues(Controller.velocity, 0f); //initalize the synch values
        }
        Debug.Log(currentPosition);
        Debug.Log(currentRotation);
    }


    public void SetMoveY(float y)
    {
        moveDirection.y = y;
    }

    // Update is called once per frame
    void Update()
    {
        //This is here to check the sendrate.  By defult its 20/sec.  I raised
        //it to 30 in "ConnectAndJoinRandom". This is clearly not the proper place,
        //it will be fixed when I figure out how
        //Debug.Log(PhotonNetwork.sendRate);

        if (m_PhotonView.isMine == false && PhotonNetwork.connected == true)
        {
            //transform.position = Vector3.Lerp(transform.position, this.currentPosition, Time.deltaTime);
            //transform.rotation = Quaternion.Lerp(transform.rotation, this.currentRotation, Time.deltaTime);
            return;
        }

        /*if (Controller.isGrounded)
        {
            moveDirection = Vector3.zero;
        }*/

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 temp = horizontal * transform.right + vertical * transform.forward;
        temp.y = 0;
        temp.Normalize();

        //this is for testing varying speed synch
        if(Input.GetButton("Sprint"))
        {
            moveDirection.x = temp.x * speed * 2f;
            moveDirection.z = temp.z * speed * 2f;
        } else
        {
        moveDirection.x = temp.x * speed;
        moveDirection.z = temp.z * speed;
        }

        if(noMove)
        {
            moveDirection.x = 0f;
            moveDirection.z = 0f;
        }

        if (!dead)
        {
            Controller.Move(moveDirection * Time.deltaTime);
        }

        //this sets the values to allow the speed synch option in the photon transform view
        m_PhotonTransformView.SetSynchronizedValues(Controller.velocity, 0f);


        RotationLateral += Input.GetAxis("Mouse X");
        RotationLongitudinal += Input.GetAxis("Mouse Y");

        transform.localRotation = Quaternion.AngleAxis(RotationLateral, Vector3.up);

        if (CameraContainer)
        {
            CameraContainer.transform.localRotation = Quaternion.AngleAxis(-RotationLongitudinal, Vector3.right);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Abilities.FireWeapon(CameraContainer.GetComponent<Camera>().transform.position, CameraContainer.GetComponent<Camera>().transform.forward);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Abilities.SecondaryAction();
        }
        if (Input.GetButtonDown("SwitchWeapons"))
        {
            Abilities.SwitchWeapon();
        }

        if (Input.GetButtonDown("Ability1"))
        {
            Abilities.Abilities.ElementAt(0).Activate();
        }

        if (Input.GetButtonDown("Ability2"))
        {
            Abilities.Abilities.ElementAt(1).Activate();
        }

        if (Input.GetButtonDown("Ability3"))
        {
            Abilities.Abilities.ElementAt(2).Activate();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

        }
        else
        {
            // Network player, receive data
            this.currentPosition = (Vector3)stream.ReceiveNext();
            this.currentRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
