using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBJ_OpenCanvas : IbjectBase {

    bool canvasOpened = false;

    public GameObject canvas;
    private GameObject interactingPlayer;
  

    void Update() {
        if (canvasOpened) { 
            if(Vector2.Distance(interactingPlayer.transform.position,this.gameObject.transform.position) > 1.5f){
                CloseCanvas();
            }

            if(interactingPlayer == null){
                operatingByOthers = false;
            }

        }

    }

    public override void Interact(GameObject player)
    {

        if (player.GetPhotonView().isMine)
        {
            if (activated && interactingPlayer == null && operatingByOthers == false)
            {
                interactingPlayer = player;

                if (canvasOpened)
                {
                    CloseCanvas();    
                }
                else
                {
                    Debug.Log("Show Canvas");
                    canvasOpened = true;
                    canvas.SetActive(true);
                    player.GetComponent<PlayerMovement>().isInteracting = true;
                    photonView.RPC("OnOpenCanvas", PhotonTargets.OthersBuffered);
                }
            }
        }

    }

    public void CloseCanvas() {
        Debug.Log("Close Canvas");
        canvasOpened = false;
        canvas.SetActive(false);
        photonView.RPC("OnCloseCanvas", PhotonTargets.OthersBuffered, interactingPlayer.GetPhotonView().viewID);
        interactingPlayer.GetComponent<PlayerMovement>().isInteracting = false;
        interactingPlayer = null;
    }

    [PunRPC]
    private void OnOpenCanvas()
    {
        operatingByOthers = false;
    }

    [PunRPC]
    private void OnCloseCanvas()
    {
        operatingByOthers = true;
    }

    

}
