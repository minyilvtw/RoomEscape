using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInput : Photon.MonoBehaviour {

    public playerBase pb;

	// Update is called once per frame
	void Update () {

            if (photonView.isMine)
            {
                CheckInput();
            }   

    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            pb.playerMovement.Jump();
        }

        /*if (Input.GetKeyDown(KeyCode.D))
        {
            pb.playerMovement.flipRight();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            pb.playerMovement.flipLeft();
        }*/

        pb.playerMovement.flip();

        if (Input.GetMouseButtonDown(0))
        {
            pb.playerMovement.Fire();
        }
    }

}
