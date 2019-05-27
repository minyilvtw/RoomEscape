using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IbjectBase : Photon.MonoBehaviour {

    public bool activated = true;
    public bool operatingByOthers = false;

    public virtual void ActivateIbject() { }

    public virtual void Interact(GameObject player){}

    public void OnDestroy() {
        Destroy(this);
    }

}
