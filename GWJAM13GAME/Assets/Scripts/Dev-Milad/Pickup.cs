using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] string itemName;

    // Dummy if statement

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    public void PickupItem()
    {
        Debug.Log($"Picked up {itemName}");
        inventory.items.Add(itemName);
        Destroy(gameObject);
    }
}