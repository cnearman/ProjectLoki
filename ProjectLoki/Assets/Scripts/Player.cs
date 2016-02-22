using UnityEngine;
using System.Collections;

public class Player : BaseClass {

    CharacterController Controller;
    PhotonView m_PhotonView;
    Vector3 moveDirection;

    public bool IsJumping { get; set; }

    public float speed;
    public float jumpSpeed;
    public float gravity;

    public float MotionInX { get; set; }
    public float MotionInY { get; set; }
    public float RotationLateral { get; set; }
    public float RotationLongitudinal { get; set; }

    public GameObject CameraContainer;

    // Use this for initialization
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        m_PhotonView = GetComponent<PhotonView>();

        m_PhotonView = GetComponent<PhotonView>();
        if (m_PhotonView.isMine)
        {
            CameraContainer.GetComponent<Camera>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PhotonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }
       
        if (Controller.isGrounded)
        {
            moveDirection = Vector3.zero;
            if (IsJumping)
            {
                moveDirection.y = jumpSpeed;
                IsJumping = false;
            }
        }

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 temp = horizontal * transform.right + vertical * transform.forward;
        temp.y = 0;
        temp.Normalize();
        moveDirection.x = temp.x * speed;
        moveDirection.z = temp.z * speed;
        moveDirection.y -= gravity * Time.deltaTime;
        Controller.Move(moveDirection * Time.deltaTime);

        RotationLateral += Input.GetAxis("Mouse X");
        RotationLongitudinal += Input.GetAxis("Mouse Y");

        transform.localRotation = Quaternion.AngleAxis(RotationLateral, Vector3.up);

        if (CameraContainer)
        {
            CameraContainer.transform.localRotation = Quaternion.AngleAxis(-RotationLongitudinal, Vector3.right);
        }
    }
}
