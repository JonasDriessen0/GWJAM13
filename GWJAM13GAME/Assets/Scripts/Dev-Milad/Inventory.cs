using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Camera playerCam;

    // Create a list for the items to be stored in (in string form)
    public List<string> items = new List<string>();

    // when pressing E on the keyboard, cast a ray and if it hits an item with the pickup class, cast the PickupItem method from the Pickup class
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Pickup pickup = hit.collider.GetComponent<Pickup>();
                if (pickup != null)
                {
                    pickup.PickupItem();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // Cast a ray from the camera and if it hits something, log it to the console
        RaycastHit hit;
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        // If the ray hits an object with a pickup class, log it to the console
        if (Physics.Raycast(ray, out hit))
        {
            Pickup pickup = hit.collider.GetComponent<Pickup>();
            if (pickup != null)
            {
                Debug.Log($"Looking at {pickup.name}");
            }
        }
    }

    public bool LookForObject (string itemName)
    {
        if (items.Contains(itemName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
