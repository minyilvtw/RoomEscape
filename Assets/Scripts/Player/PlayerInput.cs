using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : Photon.MonoBehaviour {

    
    public PlayerBase pb;

	// Update is called once per frame
	void Update () {

        if (pb.TestMode)
        {
            CheckInput();
        }
        else {
            if (photonView.isMine)
            {
                CheckInput();
            }  
        }

    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pb.PlayerMovement.Jump();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //pb.PlayerMovement.Climb();
        }

        if (Input.GetKeyUp(KeyCode.W)) {
            //pb.PlayerMovement.isClimbing = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
           // pb.PlayerMovement.Climb();
            //pb.PlayerMovement.ExitClimb();
        }

        if (Input.GetMouseButtonDown(0))
        {
            pb.PlayerMovement.Interact();
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            pb.PlayerMovement.isSlowWalking = true;
        }

        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
        
            pb.PlayerMovement.isSlowWalking = false;
        }
        
        

    }

}
