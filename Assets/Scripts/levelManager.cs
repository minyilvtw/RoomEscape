using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Photon.MonoBehaviour {

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Defeat() {
        Debug.Log("Defeated");
    }

    void Victory() {
        Debug.Log("Victory");
    }
}
