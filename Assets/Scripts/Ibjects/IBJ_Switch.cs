using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBJ_Switch : IbjectBase {

    private bool leverOn;

    public GameObject requiredItem;
    
    public GameObject leverControl;
    public GameObject leverTarget;

    public override void Interact(GameObject player) {
        if (activated)
        {
            if (player.GetPhotonView().isMine)
            {
                if (requiredItem == null || player.GetComponent<InventoryController>().CheckInventory(requiredItem.GetComponent<ItemBase>().itemName))
                {
                    if (leverOn == false)
                    {
                        photonView.RPC("ActivateTarget", PhotonTargets.AllBuffered);
                    }
                }
               
            }
        }
    }

    [PunRPC]
    protected void ActivateTarget() {
        leverOn = true;
        this.GetComponent<SpriteRenderer>().color = Color.green;
        leverTarget.GetComponent<IbjectBase>().ActivateIbject();
    }
}
