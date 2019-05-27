using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : Photon.MonoBehaviour {

    public PlayerBase pb;
    
    [Header("General Booleans")]
    public bool isInteracting = false;

    public bool isGrounded;

    public bool isSlowWalking = false;

    public bool canClimb = false;
    public bool isClimbing = false;

    [Header("General Floats")]
    public float moveSpeed = 6f;
    public float slowWalkSpeed = 1f;
    public float jumpForce = 1000f;

    [Space]
    public Rigidbody2D rb2d;
    public GameObject playerCam;

    public Text playerName;
    public Color enemyTextColor;

    public GameObject interactableZone;

    [Space]
    private Vector3 selfPos;
    private GameObject sceneCam;

    private void Awake()
    {
        
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 20;

        if (pb.TestMode)
        {
            sceneCam = GameObject.Find("Main Camera");
            sceneCam.SetActive(false);
            playerCam.SetActive(true);

            playerName.text = "Testing";
            
        }
        else {
            if (photonView.isMine)
            {
                sceneCam = GameObject.Find("Main Camera");
                sceneCam.SetActive(false);
                playerCam.SetActive(true);

                playerName.text = PhotonNetwork.playerName;
                pb.PlayerAnimation.sprite.color = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
            }
            else
            {
                playerName.text = photonView.owner.NickName;
                pb.PlayerAnimation.sprite.color = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
            }
        }
        
    }

    private void Update()
        
    {
        if (pb.TestMode)
        {
            UpdateSelfPosition();
        }
        else
        {
            if (photonView.isMine)
            {
                UpdateSelfPosition();
            }
            else
            {
                UpdateNetworkPosition();
            }
        }
    }

    private void UpdateSelfPosition()
    {
        if (!isInteracting)
        {
            // Climb Bugs
            // - jumping into ladder cause gravity change
            // - animation glitches at the top of ladder
            // - didnt set climb down (reverse effector mask arc)
            // - netcode for climbing animation
            if (canClimb)
            {
                rb2d.gravityScale = 0;

                var move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                rb2d.position += move * moveSpeed * Time.deltaTime;
                

                if (move.y != 0f)
                {
                    isClimbing = true;
                    pb.PlayerAnimation.setAnimation("Running", false, false);
                    pb.PlayerAnimation.setAnimation("Climbing", true, false);
                }

            }
            else
            {
                isClimbing = false;
                pb.PlayerAnimation.setAnimation("Climbing", false, false);
                
                rb2d.gravityScale = 4;
                var move = new Vector2(Input.GetAxis("Horizontal"), 0);

                if (move.x == 0f)
                {
                    pb.PlayerAnimation.setAnimation("Running", false, false);
                    photonView.RPC("animation_Running", PhotonTargets.AllBuffered, false);

                }
                else
                {
                    pb.PlayerAnimation.setAnimation("Running", true, false);
                    photonView.RPC("animation_Running", PhotonTargets.AllBuffered, true);

                }

                if (isSlowWalking)
                {
                    rb2d.position += move * slowWalkSpeed * Time.deltaTime;
                }
                else {
                    rb2d.position += move * moveSpeed * Time.deltaTime;
                }
            }
        }
    }

    public void Jump()
    {
        if (isGrounded && !canClimb)
        {
            rb2d.AddForce(Vector2.up * jumpForce);
        }
    }

    public void Climb()
    {
        if (canClimb)
        {
            isClimbing = true;

        }
        else {
            pb.PlayerAnimation.setAnimation("Climbing", false, false);
            isClimbing = false;
        }
        
    }


    public void Interact() {
        IbjectBase ibjectBase = null;
        pb.PlayerAnimation.setAnimation("Shooting", true, true);
        if (interactableZone.GetComponent<PlayerInteractZoneCheck>().ibject != null)
        {
            ibjectBase = interactableZone.GetComponent<PlayerInteractZoneCheck>().ibject.GetComponentInParent<IbjectBase>();
        }

        if (ibjectBase != null)
        {
            ibjectBase.Interact(this.gameObject);
        }
        else {
            Debug.Log("Nothing Interacting");
        }
    }

    public void EnterDoor() {
        Debug.Log("enter Door");
       // Enter Door Animation
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (pb.TestMode)
        {
            if (collision.gameObject.tag == "Ground")
            {
                isGrounded = true;
            }
        }
        else {
            if (photonView.isMine)
            {
                if (collision.gameObject.tag == "Ground")
                {
                    isGrounded = true;
                }
            }
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (pb.TestMode)
        {
            if (collision.gameObject.tag == "Ground")
            {
                isGrounded = false;
            }
        }
        else
        {
            if (photonView.isMine)
            {
                if (collision.gameObject.tag == "Ground")
                {
                    isGrounded = false;
                }
            }   
        }
                 
    }

    /// <summary>
    /// ///////////// NET CODE
    /// </summary>

    public void UpdateNetworkPosition()
    {
        syncTime += Time.deltaTime;
        rb2d.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    }


    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    private Vector3 syncLastPosition = Vector3.zero;

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Vector2 syncPosition = Vector2.zero;

        if (stream.isWriting)
        {
            syncPosition = rb2d.position;
            stream.Serialize(ref syncPosition);

        }
        else
        {
            stream.Serialize(ref syncPosition);

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            Vector2 heading = syncPosition - rb2d.position;
            heading.y = 0;
            syncStartPosition = rb2d.position;

            //Debug.Log("A" + syncPosition + heading / 2);
            //Debug.Log("B" + syncPosition);
            //syncEndPosition = syncPosition + heading / 2;

            syncEndPosition = syncPosition;
                     
           
        }
    }


}
