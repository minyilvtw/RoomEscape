using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_SpawnBug : MonoBehaviour {

    public void SpawnBug() {
        PhotonNetwork.InstantiateSceneObject("bug.Item", new Vector3(2f,-0.4f,0), this.transform.rotation, 0, null);
    }
}
