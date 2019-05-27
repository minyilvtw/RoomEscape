using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class playerMovement : Photon.MonoBehaviour {

    public playerBase pb;


    [Header("General Booleans")]
    public bool isGrounded;

    [Header("General Floats")]
    public float moveSpeed = 8f;
    public float jumpForce = 1000f;

    [Space]
    public SpriteRenderer sprite;
    public Rigidbody2D rigidBody;
    public GameObject playerCam;
    public GameObject arrowPrefab;
    public Text playerName;
    public GameObject firePosLeft;
    public GameObject firePosRight;
    public Camera playerCam2;
    public Color enemyTextColor;
    public GameObject explosion_image_prefab;

    [Space]
    private Vector3 selfPos;
    private GameObject sceneCam;

    // for fire delay
    private float timestamp = 0;

    private void Awake()
    {
        PhotonNetwork.sendRate = 20;
        PhotonNetwork.sendRateOnSerialize = 20;
        if(photonView.isMine) {
            sceneCam = GameObject.Find("Main Camera");
            sceneCam.SetActive(false);
            playerCam.SetActive(true);
                    
            playerName.text = PhotonNetwork.playerName;
            sprite.color = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
        } else {
            playerName.text = photonView.owner.NickName;
            sprite.color = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));

        }
    }

    private void Update()
        
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

    private void UpdateSelfPosition()
    {
        var move = new Vector2(Input.GetAxis("Horizontal"), 0);
        rigidBody.position += move * moveSpeed * Time.deltaTime;

    }

    public void flipRight()
    {
        sprite.flipX = false;
        photonView.RPC("OnSpriteFlipFalse", PhotonTargets.Others);
    }

    public void flipLeft()
    {
        sprite.flipX = true;
        photonView.RPC("OnSpriteFlipTrue", PhotonTargets.Others);
    }

    public void flip(){
        if (Input.mousePosition.x > (Screen.width / 2)){
            flipRight();
        } else {
            flipLeft();
        }
    }

    public void Fire()
    {
        if (pb.playerStatus.ammo < 1)
        {
            return;
        }
        pb.playerStatus.ammo--;

        photonView.RPC("changeAmmo", PhotonTargets.AllBuffered, -1f);

        if (sprite.flipX == false)
        {
               
                object[] data = new object[2];
                data[0] = Input.mousePosition.x - playerCam2.WorldToScreenPoint(transform.position).x;
                data[1] = Input.mousePosition.y - playerCam2.WorldToScreenPoint(transform.position).y;
                Debug.Log("data0" + data[0]);
                Debug.Log("data1" + data[1]);
                GameObject obj = PhotonNetwork.Instantiate(arrowPrefab.name, 
                firePosRight.transform.position, Quaternion.identity, 0, data);
            
        }
        else
        {
                Camera cam = GetComponent<Camera>();
                object[] data = new object[2];
                data[0] = Input.mousePosition.x - playerCam2.WorldToScreenPoint(transform.position).x;
                data[1] = Input.mousePosition.y - playerCam2.WorldToScreenPoint(transform.position).y;
         
                GameObject obj = PhotonNetwork.Instantiate(arrowPrefab.name, 
                                                           firePosLeft.transform.position, Quaternion.identity, 0, data);
                obj.GetComponent<PhotonView>().RPC("changeDirection_Left", PhotonTargets.AllBuffered);
        }
    }

    public void Fire2(){
        
        float timebetweenshots = 1f;
        if (Time.time >= timestamp)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 2);
            PhotonNetwork.Instantiate(explosion_image_prefab.name, transform.position, Quaternion.identity, 0);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                PhotonView target = hitColliders[i].gameObject.GetComponent<PhotonView>();

                if (target != null && (!target.isMine || target.isSceneView))
                {

                    if (hitColliders[i].tag == "Player")
                    {
                        hitColliders[i].GetComponent<playerStatus>().takeDMG();
                    }
                }
            }
            timestamp = Time.time + timebetweenshots;
        }
    }


    public void Jump()
    {
        if (isGrounded)
        {
            rigidBody.AddForce(Vector2.up * jumpForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonView.isMine)
            {
                if (collision.gameObject.tag == "Ground")
                {
                    isGrounded = true;
                }
            }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (photonView.isMine)
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

    public void UpdateNetworkPosition()
    {
        //transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 15);
        syncTime += Time.deltaTime;
        rigidBody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    }


    [PunRPC]
    private void OnSpriteFlipTrue()
    {
        sprite.flipX = true;
    }
    [PunRPC]
    private void OnSpriteFlipFalse()
    {
        sprite.flipX = false;
    }


    //private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    Vector3 syncPos = Vector3.zero;
    //    if (stream.isWriting)
    //    {
    //        stream.SendNext(transform.position);
    //    }
    //    else
    //    {
    //        selfPos = (Vector3)stream.ReceiveNext();
    //    }
    //}

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
            syncPosition = rigidBody.position;
            stream.Serialize(ref syncPosition);

        }
        else
        {
            stream.Serialize(ref syncPosition);

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            Vector2 heading = syncPosition - rigidBody.position;
            heading.y = 0;
            syncStartPosition = rigidBody.position;


            Debug.Log("A" + syncPosition + heading / 2);
            Debug.Log("B" + syncPosition);
            //syncEndPosition = syncPosition + heading / 2;

            syncEndPosition = syncPosition;
                     

        }
    }


}
