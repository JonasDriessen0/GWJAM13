using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombComponentsManager : MonoBehaviour
{
    [SerializeField] private List<Transform> componentPositions;
    [SerializeField] private List<GameObject> availableComponents;

    private void Start()
    {
        SpawnComponents();
    }

    private void SpawnComponents()
    {
        foreach (var spawnPoint in componentPositions)
        {
            var randomComponent = Random.Range(0, availableComponents.Count);
            Instantiate(availableComponents[randomComponent], spawnPoint);
            availableComponents.RemoveAt(randomComponent);
        }
    }
}
