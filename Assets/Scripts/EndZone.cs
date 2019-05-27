using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPoints");
        collision.transform.position = Vector3.Lerp(collision.transform.position, spawnPoints[0].transform.position, Time.deltaTime * 40);
    }
}
