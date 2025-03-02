using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    Inventory inventory;
    string itemName;

    // Dummy if statement

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    private void PickupItem()
    {
        inventory.items.Add(itemName);
        Destroy(gameObject);
    }
}