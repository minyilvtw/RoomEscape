using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class ICanvas_PasscodePad : Photon.MonoBehaviour {

    public Text textField;
    public GameObject unlockTarget;

    public string passcode;
    public int maxInputLength;

	
    public void EnterDigit(int i){
        if (textField.text.Length < maxInputLength )
        {
            textField.text += i;
            
        } else {
            ClearPasscode();  
        }
    
    }

    public void ClearPasscode() {
        textField.text = "";     
    }

    public void CheckPasscode() {

        if (textField.text.Equals(passcode))
        {
            Debug.Log("Correct");
            CorrectAction();
            ClearPasscode();  
            GetComponentInParent<IBJ_OpenCanvas>().CloseCanvas();

        } else {
            ClearPasscode();  
        }
    }

    public void CorrectAction() {
        PhotonNetwork.InstantiateSceneObject("key.Item", unlockTarget.transform.position, this.transform.rotation, 0, null);
        unlockTarget.SetActive(true);
    }
}
