using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBJ_ClickDoor : IbjectBase {

    bool doorOpened = false;
    public float moveUpDistance;

    public override void Interact(GameObject player) { 
        if(activated){

            if (doorOpened)
            {
                photonView.RPC("OnCloseDoor", PhotonTargets.AllBufferedViaServer);
            }
            else
            {
                photonView.RPC("OnOpenDoor", PhotonTargets.AllBufferedViaServer);
                
            }
        }
        
    }

    [PunRPC]
    private void OnOpenDoor() {
        doorOpened = true;
        this.transform.position += new Vector3(0, moveUpDistance, 0);
    }

    [PunRPC]
    private void OnCloseDoor()
    {
        this.transform.position += new Vector3(0, -moveUpDistance, 0);
        doorOpened = false;
    }

}
