using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombComponentsManager : MonoBehaviour
{
    [SerializeField] private List<Transform> componentPositions;
    [SerializeField] private List<GameObject> availableComponents;
    private List<GameObject> spawnedComponents = new List<GameObject>();
    private int currentComponentIndex = 0;

    private void Start()
    {
        SpawnComponents();
        RevealNextComponent(); // Start by revealing the first component
    }

    private void SpawnComponents()
    {
        foreach (var spawnPoint in componentPositions)
        {
            int randomIndex = Random.Range(0, availableComponents.Count);
            GameObject component = Instantiate(availableComponents[randomIndex], spawnPoint);
            availableComponents.RemoveAt(randomIndex);
            
            spawnedComponents.Add(component); // Store the spawned component
        }
    }

    public void RevealNextComponent()
    {
        if (currentComponentIndex < spawnedComponents.Count)
        {
            Animator anim = spawnedComponents[currentComponentIndex].GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Activate");
            }
            currentComponentIndex++;
        }
    }
}