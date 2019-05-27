using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IBJ_MultiDoor : IbjectBase {

    public Sprite buggedSprite;

    public GameObject gateOne;
    public GameObject gateTwo;
    public GameObject gateThree;

    public GameObject requiredItem;

    public override void Interact(GameObject player)
    {
        Debug.Log(GetComponent<SpriteRenderer>().sprite);
        if (activated)
        {
            if (player.GetComponent<InventoryController>().CheckInventory(requiredItem.GetComponent<ItemBase>().itemName))
            {

                InvokeRepeating("MoveGate", 1f, 1.5f);
                GetComponent<SpriteRenderer>().sprite = buggedSprite;
            }
            else {
                MoveGate();
            }

        }
    }

    void MoveGate() {
        gateOne.transform.localPosition = new Vector3(0, 0, 0);
        gateTwo.transform.localPosition = new Vector3(1.5f, 0, 0);
        gateThree.transform.localPosition = new Vector3(3f, 0, 0);

        int random = Random.Range(0, 6);
        Debug.Log(random);
        switch (random)
        {
            case 0:
                gateOne.transform.position += new Vector3(0, 1.5f, 0);
                break;
            case 1:
                gateTwo.transform.position += new Vector3(0, 1.5f, 0);
                break;
            case 2:
                gateThree.transform.position += new Vector3(0, 1.5f, 0);
                break;
            case 3:
                gateOne.transform.position += new Vector3(0, 1.5f, 0);
                gateTwo.transform.position += new Vector3(0, 1.5f, 0);
                break;
            case 4:
                gateOne.transform.position += new Vector3(0, 1.5f, 0);
                gateThree.transform.position += new Vector3(0, 1.5f, 0);
                break;
            case 5:
                gateTwo.transform.position += new Vector3(0, 1.5f, 0);
                gateThree.transform.position += new Vector3(0, 1.5f, 0);
                break;
            default:
                break;
        }      

    }
}
