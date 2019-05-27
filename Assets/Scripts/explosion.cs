using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Photon.MonoBehaviour {
    
	
	void Awake () {
        this.GetComponent<PhotonView>().RPC("destroyObj", PhotonTargets.AllBuffered);
	}
	
    [PunRPC]
    private void destroyObj()
    {
        Destroy(gameObject, 0.3f);
    }
}
