using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireCable : MonoBehaviour
{
    public bool isCorrectWire;
    private bool isCut;

    [SerializeField] private GameObject intactWire;  
    [SerializeField] private GameObject cutWire;
    
    public void CutWire()
    {
        if (isCut) return;
        isCut = true;
        
        Debug.Log($"{gameObject.name} was cut. Correct wire? {isCorrectWire}");
        
        if (intactWire != null) intactWire.SetActive(false);
        if (cutWire != null) cutWire.SetActive(true);
        
        // Notify puzzle manager
        CableComponent.Instance.WireCut(this);
    }
}
