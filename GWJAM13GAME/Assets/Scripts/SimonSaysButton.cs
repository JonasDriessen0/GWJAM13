using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimonSaysButton : MonoBehaviour, IClickable
{
    [SerializeField] private List<GameObject> buttons;
    [SerializeField] private List<GameObject> lights;

    private void Start()
    {
        foreach (var light in lights)
        {
            light.SetActive(false);
        }
    }

    public void OnClick()
    {
        Debug.Log(gameObject.name + " was clicked!");
        // Add your custom logic here (e.g., change color, open a menu, etc.)
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
}