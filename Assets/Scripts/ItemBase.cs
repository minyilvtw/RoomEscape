using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : Photon.MonoBehaviour {

    public string itemName;

    private void OnTriggerEnter2D(Collider2D collision)
    {

            if (collision.tag == "Player")
            {
                PlayerStatus player = collision.gameObject.GetComponent<PlayerStatus>();

                    PickUpObject(player);
            }

    }

    void PickUpObject(PlayerStatus player)
    {
        player.photonView.RPC("PickUpItem", PhotonTargets.AllBuffered, itemName);
        photonView.RPC("OnPickUp", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    protected void OnPickUp()
    {
        Destroy(this.gameObject);
    }

}
