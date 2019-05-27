using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class playerMovement : Photon.MonoBehaviour {

    public bool devTesting = false;

    public PhotonView photonView;


    public bool isGrounded;

    public SpriteRenderer sprite;
    public Rigidbody2D rigidBody;
    public GameObject playerCam;

    public Text playerName;

    public float moveSpeed = 8f;
    public float jumpForce = 1000f;

    private Vector3 selfPos;

    private GameObject sceneCam;

    private void Awake()
    {
        if(!devTesting && photonView.isMine)
        {
            sceneCam = GameObject.Find("Main Camera");
            sceneCam.SetActive(false);
            playerCam.SetActive(true);

            playerName.text = PhotonNetwork.playerName;
        } else
        {
            playerName.text = photonView.owner.name;
        }
    }

    private void Update()
    {

        if (!devTesting)
        {
            if (photonView.isMine)
            {
                checkInput();
            }
            else
            {
                smoothNetMovement();
            }
        } else
        {
            checkInput();
        }
       
    }

    private void checkInput()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0);
        transform.position += move * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Debug.Log("JUMMPPP");
            Jump();
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            sprite.flipX = false;
            photonView.RPC("onSpriteFlipFalse", PhotonTargets.Others);
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            sprite.flipX = true;
            photonView.RPC("onSpriteFlipTrue", PhotonTargets.Others);
        }
    }

    void Jump()
    {
        rigidBody.AddForce(Vector2.up * jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!devTesting)
        {
            if (photonView.isMine)
            {
                if (collision.gameObject.tag == "Ground")
                {
                    isGrounded = true;
                }
            }
        } else
        {
            if (collision.gameObject.tag == "Ground")
            {
                isGrounded = true;
            }
        }
       
       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!devTesting)
        {
            if (photonView.isMine)
            {
                if (collision.gameObject.tag == "Ground")
                {
                    isGrounded = false;
                }
            }
        } else
        {
            if (collision.gameObject.tag == "Ground")
            {
                isGrounded = false;
            }
        }
        
            
    }







    /// <summary>
    /// ///////////// NET CODE
    /// </summary>

    private void smoothNetMovement()
    {
        // if we are not controlling, see other player movement
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
    }


    [PunRPC]
    private void onSpriteFlipTrue()
    {
        sprite.flipX = true;
    }
    [PunRPC]
    private void onSpriteFlipFalse()
    {
        sprite.flipX = false;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting) // means this is our local player, send stream (position etc)
        {
            stream.SendNext(transform.position);
        } else
        {
            selfPos = (Vector3)stream.ReceiveNext();
        }
    }

}
