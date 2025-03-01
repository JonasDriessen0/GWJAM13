using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClickableObject : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        Debug.Log(gameObject.name + " was clicked!");
        // Add your custom logic here (e.g., change color, open a menu, etc.)
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
}