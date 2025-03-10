using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysComponent : MonoBehaviour
{
    [SerializeField] private List<SimonSaysButton> buttons;
    [SerializeField] private List<GameObject> lights;

    public bool hasCompleted;
    
    private List<List<int>> patterns = new List<List<int>> 
    {
        new List<int> { 0, 1, 2 },
        new List<int> { 3, 2, 1, 0 },
        new List<int> { 1, 3, 0, 2, 1 },
        new List<int> { 0, 1, 0, 3, 2, 3 },
        new List<int> { 2, 3, 1, 0, 3, 1, 2 }
    };

    private List<int> currentPattern = new List<int>();
    private List<int> playerInput = new List<int>();
    private int currentPatternIndex = 0; 
    private bool isPlayerTurn = false;

    private void Start()
    {
        ResetGame();
    }

    private void ResetGame()
    {
        Debug.Log("Resetting game...");
        currentPatternIndex = 0;
        playerInput.Clear();
        currentPattern.Clear(); // Clear current pattern to avoid old inputs
        isPlayerTurn = false;

        foreach (var light in lights)
        {
            light.SetActive(false);
        }

        foreach (var button in buttons)
        {
            if (button == null)
            {
                Debug.LogError("A button reference is missing! Assign the script reference.");
            }
            else
            {
                button.OnButtonPressed -= HandleButtonPress; // Ensure no duplicate events
                button.OnButtonPressed += HandleButtonPress;
            }
        }

        StartCoroutine(FlashPattern(patterns[currentPatternIndex])); // Start first pattern
    }

    private void HandleFlash(SimonSaysButton button)
    {
        button.Flash();
    }

    private void HandleButtonPress(SimonSaysButton button)
    {
        if (!isPlayerTurn) return; 

        int index = buttons.IndexOf(button);
        if (index == -1) return;

        HandleFlash(button);
        playerInput.Add(index);

        if (playerInput[playerInput.Count - 1] != currentPattern[playerInput.Count - 1])
        {
            Debug.Log("Wrong input! Resetting game...");
            StartCoroutine(FlashFailureEffect());
            return;
        }

        Debug.Log("Correct button!");

        if (playerInput.Count == currentPattern.Count)
        {
            Debug.Log("Pattern completed!");
            StartCoroutine(FlashSuccessEffect());
        }
    }

    private IEnumerator FlashPattern(List<int> buttonIndexes)
    {
        Debug.Log("Starting Flash Pattern...");
        currentPattern = new List<int>(buttonIndexes);
        playerInput.Clear(); // Reset player input for new round
        isPlayerTurn = false;

        yield return new WaitForSeconds(1f);

        foreach (int index in buttonIndexes)
        {
            if (index >= 0 && index < buttons.Count)
            {
                HandleFlash(buttons[index]);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                Debug.LogError($"Invalid button index: {index}");
            }
        }

        Debug.Log("Player's turn! Repeat the pattern.");
        isPlayerTurn = true;
    }

    private IEnumerator FlashSuccessEffect()
    {
        isPlayerTurn = false;

        yield return new WaitForSeconds(0.3f);
        
        for (int i = 0; i < 2; i++)
        {
            foreach (var button in buttons)
            {
                button.Flash();
            }
            yield return new WaitForSeconds(0.3f);
        }

        if (currentPatternIndex < lights.Count)
        {
            lights[currentPatternIndex].SetActive(true);
        }

        playerInput.Clear();
        currentPatternIndex++;

        if (currentPatternIndex < patterns.Count)
        {
            StartCoroutine(FlashPattern(patterns[currentPatternIndex]));
        }
        else
        {
            hasCompleted = true;
            Debug.Log("You have completed all patterns!");
        }
    }

    private IEnumerator FlashFailureEffect()
    {
        isPlayerTurn = false;

        yield return new WaitForSeconds(0.3f);
        
        for (int i = 0; i < 2; i++)
        {
            foreach (var button in buttons)
            {
                button.FlashRed();
            }
            yield return new WaitForSeconds(0.3f);
        }
        
        yield return new WaitForSeconds(0.5f);

        ResetGame();
    }
}
