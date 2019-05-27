using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBJ_Teleporter : IbjectBase {

    public GameObject indicator;

    public GameObject pointA;
    public GameObject pointB;

    public override void ActivateIbject(){
        activated = true;
        indicator.GetComponent<SpriteRenderer>().color = Color.green;
    }


    public override void Interact(GameObject player) {
        if (activated) {
            // IMPORTANT:Fix this condition
            if (player.GetPhotonView().isMine) {
                if (Vector2.Distance(pointA.transform.position, player.transform.position)
                    > Vector2.Distance(pointB.transform.position, player.transform.position))
                {
                    // ADD transition and move Scene here
                 
                    player.transform.position = pointA.transform.position;
                    player.GetComponent<PlayerMovement>().EnterDoor();
                }
                else {
                    player.transform.position = pointB.transform.position;
                    player.GetComponent<PlayerMovement>().EnterDoor();

                }
            }
        }
    }

}
