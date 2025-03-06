using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireCable : MonoBehaviour
{
    public bool isCorrectWire;
    private bool isCut;

    public void CutWire()
    {
        if (isCut) return;
        isCut = true;

        // Visual Feedback
        GetComponent<MeshRenderer>().material.color = Color.gray;
        GetComponent<Collider>().enabled = false;

        // Notify puzzle manager
        CableComponent.Instance.WireCut(this);
    }
}
