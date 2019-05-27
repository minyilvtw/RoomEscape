using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class photonUserRegister : MonoBehaviour {

    private bool alreadyRegistered = false;

    public GameObject objectParent;
    public InputField nameInput;
    public GameObject createButton;

    private void Awake()
    {
            checkRegister();
    }

    void checkRegister()
    {
        if (!alreadyRegistered)
        {
            objectParent.SetActive(true);
        }
    }

    public void nameInputChange()
    {
        if(nameInput.text.Length >= 2)
        {
            createButton.SetActive(true);
        }
        else
        {
            createButton.SetActive(false);
        }
    }

    public void createName()
    {
        PhotonNetwork.playerName = nameInput.text;
        objectParent.SetActive(false);

        Debug.Log("This machine name is " + PhotonNetwork.playerName);

    }

}
