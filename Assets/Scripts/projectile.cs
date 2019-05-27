using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : Photon.MonoBehaviour {

    public bool moveDirectionLeft = false;
    public float moveSpeed = 8f;
    public float destroyTime = 1f;
    private Vector2 Direction;

    [PunRPC]
    public void changeDirection_Left()
    {
        moveDirectionLeft = true;
    }

    private void Awake()
    {
        object[] data = photonView.instantiationData;
        Direction.x = (float)data[0];
        Direction.y = (float)data[1];
        GetComponent<Rigidbody2D>().velocity = Direction.normalized * 10;
    }
    void Update()
    {
        /*if (!moveDirectionLeft)
        {
            
            transform.Translate(Direction.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Direction.normalized * moveSpeed * Time.deltaTime);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!photonView.isMine)
        {
            return;
        }

       
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if (target != null && (!target.isMine || target.isSceneView))
        {

            if (collision.tag == "Player")
            {
                collision.GetComponent<PlayerStatus>().takeDMG(this.photonView.owner);
                this.GetComponent<PhotonView>().RPC("destroyObj", PhotonTargets.All);
            }
        }

    }

    [PunRPC]
    private void destroyObj()
    {
        Destroy(gameObject);
    }

}
