using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : Photon.MonoBehaviour {

    public PlayerBase pb;
    public InventoryController inventory;

    public GameObject localCanvas;
    public GameObject otherCanvas;

    public Text pingText;
    public Text scoreText;

    public Image otherHealthBar;
    public Image otherAmmoBar;

    public float health;
    public float ammo;

    private float selfHealth;
    private Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        
        if (pb.TestMode)
        {
            health = 1f;
            ammo = 3;
        }
        else {
            if (photonView.isMine)
            {
                health = 1f;
                ammo = 3;
            }
        }

         setCorrectCanvas();
    }

    void setCorrectCanvas()
    {
        if (pb.TestMode)
        {
            localCanvas.SetActive(true);
        }
        else
        {
            if (photonView.isMine)
            {
                localCanvas.SetActive(true);
            }
        }
        
    }

    private float lastUpdate;

    private void Update()
    {
        if(pb.TestMode){
            otherHealthBar.fillAmount = health;
            otherAmmoBar.fillAmount = ammo / 3;

            if (Time.time >= lastUpdate)
            {
                //pingText.text = "Ping: " + PhotonNetwork.GetPing();
                scoreText.text = sortPlayerRank();
                lastUpdate = Time.time + 1f;
            }
        } else {
            if (photonView.isMine)
            {
                otherHealthBar.fillAmount = health;
                otherAmmoBar.fillAmount = ammo / 3;

                if (Time.time >= lastUpdate)
                {
                    pingText.text = "Ping: " + PhotonNetwork.GetPing();
                    scoreText.text = sortPlayerRank();
                    lastUpdate = Time.time + 1f;
                }

            }
            else
            {
                UpdateNetworkStatus();

            }
        }
       

    }

    string sortPlayerRank()
    {
       
        string formattedRank = "";
        //PhotonPlayer[] rankedList = PhotonNetwork.playerList
        for (int i = 0; i < PhotonNetwork.playerList.Length; i ++)
        {
            formattedRank = formattedRank + PhotonNetwork.playerList[i].NickName + "\t\t\t" + PhotonNetwork.playerList[i].GetScore() + "\n"; 
        }
        
        return formattedRank;
    }

    void UpdateNetworkStatus()
    {
        otherHealthBar.fillAmount = selfHealth;
        
    }

    public void takeDMG(PhotonPlayer fromPlayer)
    {

            health -= 0.5f;

            photonView.RPC("changeHealth", PhotonTargets.AllBufferedViaServer, health);

            if (health <= 0)
            {
                fromPlayer.AddScore(1);
                health = 1;
                GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPoints");
                int random = Random.Range(0, spawnPoints.Length);
                
                Debug.Log("DIED");
                photonView.RPC("Death", PhotonTargets.AllViaServer, random);
            }
             

    }

    [PunRPC]
    void Death(int pos)
    {
        playerAnimator.SetBool("Die", true);
        health = 1f;
        selfHealth = health;

        otherHealthBar.fillAmount = health;
        otherAmmoBar.fillAmount = 1f;
        ammo = 3;

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPoints");
        this.transform.position = Vector3.Lerp(transform.position, spawnPoints[pos].transform.position, Time.deltaTime * 50);
        //playerAnimator.SetBool("Die", false);
        //pb.playerMovement.selfPos = this.transform.position;
        
    }

    [PunRPC]
    public void changeHealth(float amount)
    {
        Debug.Log("CHANGED");
        changeHealthAmount(amount);
    }

    public void changeHealthAmount(float amount)
    {
        health = amount;
    }

    [PunRPC]
    public void PickUpItem(string item)
    {
        inventory.AddItem(item);
    }

}
