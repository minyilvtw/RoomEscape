using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerStatus : Photon.MonoBehaviour {

    public playerBase pb;

    public GameObject localCanvas;
    public GameObject otherCanvas;

    public Text pingText;
    public Text scoreText;

    public Image otherHealthBar;
    public Image otherAmmoBar;

    public float health;
    public float ammo;

    private float selfHealth;
    

    private void Awake()
    {
        if (photonView.isMine)
        {
            health = 1f;
            ammo = 3;
        }

         setCorrectCanvas();
    }

    void setCorrectCanvas()
    {
        if (photonView.isMine)
        {
            localCanvas.SetActive(true);
        }
    }

    private float lastUpdate;

    private void Update()
    {
        if (photonView.isMine)
        {
            otherHealthBar.fillAmount = health;
            otherAmmoBar.fillAmount = ammo/3;

            if (Time.time >= lastUpdate)
            {
                pingText.text = "Ping: " + PhotonNetwork.GetPing();
                scoreText.text = sortPlayerRank();
                lastUpdate = Time.time + 1f;
            }
            
        } else
        {
            UpdateNetworkStatus();

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
        //health = selfHealth;
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
        health = 1f;
        selfHealth = health;

        otherHealthBar.fillAmount = health;
        otherAmmoBar.fillAmount = 1f;
        ammo = 3;

        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPoints");
        this.transform.position = Vector3.Lerp(transform.position, spawnPoints[pos].transform.position, Time.deltaTime * 50);
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
    public void changeAmmo(float amount)
    {
        changeAmmoAmount(amount);
    }

    public void changeAmmoAmount(float amount)
    {

        otherAmmoBar.fillAmount += amount/3;

    }


    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting) // means this is our local player, send stream (position etc)
        {
            stream.SendNext(health);
        }
        else
        {
            selfHealth = (float)stream.ReceiveNext();
        }
    }





}
