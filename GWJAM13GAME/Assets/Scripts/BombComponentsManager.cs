using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombComponentsManager : MonoBehaviour
{
    [SerializeField] private List<Transform> componentPositions;
    [SerializeField] private List<GameObject> availableComponents;
    private List<GameObject> spawnedComponents = new List<GameObject>();
    private CountdownTimer countdownTimer;

    private void Start()
    {
        countdownTimer = FindObjectOfType<CountdownTimer>();
        SpawnComponents();
        StartCoroutine(CheckCompletionRoutine());
    }

    private void SpawnComponents()
    {
        foreach (var spawnPoint in componentPositions)
        {
            int randomIndex = Random.Range(0, availableComponents.Count);
            GameObject component = Instantiate(availableComponents[randomIndex], spawnPoint);
            availableComponents.RemoveAt(randomIndex);
            
            spawnedComponents.Add(component);
        }
    }

    private IEnumerator CheckCompletionRoutine()
    {
        while (true)
        {
            bool allCompleted = true;
            
            foreach (var component in spawnedComponents)
            {
                if (!IsComponentCompleted(component))
                {
                    allCompleted = false;
                    break;
                }
            }

            if (allCompleted)
            {
                Time.timeScale = 0;
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private bool IsComponentCompleted(GameObject component)
    {
        var fuse = component.GetComponent<FuseComponent>();
        var simonSays = component.GetComponent<SimonSaysComponent>();
        var signalMinigame = component.GetComponent<SignalMinigame>();
        var numpad = component.GetComponent<NumpadSystem>();
        
        return (fuse != null && fuse.hasCompleted) ||
               (simonSays != null && simonSays.hasCompleted) ||
               (signalMinigame != null && signalMinigame.hasCompleted) ||
               (numpad != null && numpad.hasCompleted);
    }
}