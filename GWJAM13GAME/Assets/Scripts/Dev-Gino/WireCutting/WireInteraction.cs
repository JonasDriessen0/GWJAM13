using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireInteraction : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                WireCable wire = hit.collider.GetComponent<WireCable>();
                if (wire != null)
                {
                    wire.CutWire();
                }
            }
        }
    }
}