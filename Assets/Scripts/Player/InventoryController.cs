using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    // IMPORTANT: List is not performant, change in future
    public List<string> inventoryItems;

    [ContextMenu("Print Inventory")]
    void PrintInventory() {
        foreach (string x in inventoryItems) {
            Debug.Log(x);
        }
    }

    public bool CheckInventory(string item) { 
        return inventoryItems.Contains(item);
    }

    public void AddItem(string item) {
        if (!CheckInventory(item)) {
            inventoryItems.Add(item);
        }
    }

    public void RemoveItem(string item)
    {
        if (!CheckInventory(item))
        {
            inventoryItems.Remove(item);
        }
    }
}
