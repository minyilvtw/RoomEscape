using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class photonHandler : MonoBehaviour {

    public photonButton photonB;

    public GameObject mainPlayer;

    private void Awake()
    {
        DontDestroyOnLoad(this.transform);
        
        // Changing network lag and smooth
        PhotonNetwork.sendRate = 10;
        PhotonNetwork.sendRateOnSerialize = 10;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public void createNewRoom()
    {
        PhotonNetwork.CreateRoom(photonB.createRoomInput.text, new RoomOptions() { MaxPlayers = 8 }, null);

    }

    public void joinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        PhotonNetwork.JoinOrCreateRoom(photonB.joinRoomInput.text, roomOptions, TypedLobby.Default);
    }

    public void joinTest()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        PhotonNetwork.JoinOrCreateRoom("1", roomOptions, TypedLobby.Default);
    }

    public void moveScene()
    {
        PhotonNetwork.LoadLevel("Level-0");
    }

    private void OnJoinedRoom()
    {
        moveScene();
        Debug.Log("Joined Room");
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level-0")
        {
            spawnPlayer();
        }
    }

    private void spawnPlayer()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPoints");
        int random = Random.Range(0, spawnPoints.Length);
        PhotonNetwork.Instantiate(mainPlayer.name, spawnPoints[random].transform.position, mainPlayer.transform.rotation, 0);

    }

}
