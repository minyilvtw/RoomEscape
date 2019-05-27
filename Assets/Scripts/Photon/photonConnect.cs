using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photonConnect : MonoBehaviour {

    public string versionName = "0.1";

    public GameObject sectionView1, sectionView2, sectionView3;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);
        //PhotonNetwork.ConnectToMaster("172.19.10.86", 5055,"02",versionName);
        

        Debug.Log("Connecting...");
        Debug.Log("IP:"+PhotonNetwork.ServerAddress);
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        Debug.Log("We are conencted to master");
    }

    private void OnJoinedLobby()
    {
        sectionView1.SetActive(false);
        sectionView2.SetActive(true);
        
        Debug.Log("On Joined Lobby");
    }

    private void OnDisconnectedFromPhoton()
    {
        sectionView1.SetActive(false);
        sectionView2.SetActive(false);
        sectionView3.SetActive(true);


        Debug.Log("Disconnected from photon services");
    }

    private void OnFailedToConnectToPhoton()
    {

    }


}
