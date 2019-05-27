using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractZoneCheck : MonoBehaviour {

    public GameObject ibject = null;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ibjects") {
            ibject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        ibject = null;
    }

}
