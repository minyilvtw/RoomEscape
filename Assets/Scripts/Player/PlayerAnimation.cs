using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : Photon.MonoBehaviour {

    public PlayerBase pb;

    public Animator playerAnimator;
    public SpriteRenderer sprite;

    private void Update() {
        pb.PlayerAnimation.flipUpdate();
    }

    public void setAnimation(string animationString, bool active, bool isTrigger) {

        if (isTrigger)
        {
            playerAnimator.SetTrigger(animationString);
            
        } else {
            playerAnimator.SetBool(animationString, active);
        }

    }

    public void flipRight()
    {
        sprite.flipX = false;
        photonView.RPC("OnSpriteFlip", PhotonTargets.Others, false);

        pb.PlayerMovement.interactableZone.GetComponent<BoxCollider2D>().offset = new Vector3(0.3f, 0, 0);
    }

    public void flipLeft()
    {
        sprite.flipX = true;
        photonView.RPC("OnSpriteFlip", PhotonTargets.Others, true);

        pb.PlayerMovement.interactableZone.GetComponent<BoxCollider2D>().offset = new Vector3(-0.3f, 0, 0);
    }

    public void flipUpdate()
    {
        if (!pb.PlayerMovement.isClimbing)
        {
            if (Input.mousePosition.x > (Screen.width / 2))
            {
                flipRight();
            }
            else
            {
                flipLeft();
            }
        }
       
    }

    /// <summary>
    /// ///////////// NET CODE
    /// </summary>

    [PunRPC]
    private void OnSpriteFlip(bool active)
    {
        sprite.flipX = active;
    }

    [PunRPC]
    private void animation_Running(bool active)
    {
        playerAnimator.SetBool("Running", active);
    }

    [PunRPC]
    private void animation_Climbing(bool active)
    {
        playerAnimator.SetBool("Climbing", active);
    }

    [PunRPC]
    private void animation_Fire_Trigger()
    {
        playerAnimator.SetTrigger("Shooting");
    }
}
