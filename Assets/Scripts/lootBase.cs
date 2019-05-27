using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBase : Photon.MonoBehaviour {

    public float cooldown = 10f;
    private float respawnTime;

    private Vector3 spawnPosition;
    private bool isActive = true;
    

    void Awake()
    {
        spawnPosition = this.transform.position;
    }

    void Update()
    {
            if (!isActive)
            {
                if (Time.time >= respawnTime)
                {
                    isActive = true;
                    this.GetComponent<Renderer>().enabled = true;
                }
            }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
        {
            if (collision.tag == "Player")
            {
                PlayerStatus player = collision.gameObject.GetComponent<PlayerStatus>();

                if (CanPickUp(player))
                {
                    PickUpObject(player);
                }
            }
        }
        
    }

    bool CanPickUp(PlayerStatus player)
    {
        if(player.ammo < 3)
        {
            return true;
        } else
        {
            return false;
        }
    }

    void PickUpObject(PlayerStatus player)
    {

        player.ammo++;
        player.photonView.RPC("changeAmmo", PhotonTargets.AllBuffered, 1f );

        player.photonView.RPC("OnPickUp", PhotonTargets.AllBuffered, player.photonView.viewID);
    }

    //[PunRPC]
    //protected void OnPickUp(int viewID)
    //{
    //    respawnTime = Time.time + cooldown;

    //    isActive = false;
    //    this.GetComponent<Renderer>().enabled = false;
        
    //    //PhotonNetwork.Destroy(this.gameObject);
     
    //}

}
