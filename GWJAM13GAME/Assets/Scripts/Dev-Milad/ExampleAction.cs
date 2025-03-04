using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExampleAction : MonoBehaviour
{
    // Grab the items list from the inventory class
    [SerializeField] Inventory inventory;

    // if Inventory contains "Cube" then do stuff
    private void Update()
    {
        if (inventory.items.Contains("Cube")) // Replace Cube with whatever item is needed.
        {
            // Do stuff
        }
    }
}
