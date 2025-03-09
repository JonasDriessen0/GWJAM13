using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FuseComponent : MonoBehaviour
{
    [Header("Fuse Settings")]
    [SerializeField] private GameObject[] fuses = new GameObject[3]; // The three fuse objects
    [SerializeField] private float normalVoltageMin = 110f;
    [SerializeField] private float normalVoltageMax = 130f;
    [SerializeField] private float brokenVoltageMin = 30f;
    [SerializeField] private float brokenVoltageMax = 60f;
    [SerializeField] private float fuseRemovalTime = 2f; // Time to hold right click to remove a fuse
    [SerializeField] private TMP_Text voltageDisplayText; // TMP text object for voltage display

    private int brokenFuseIndex; // Which fuse is broken
    private float[] fuseVoltages = new float[3];
    private bool[] fuseRemoved = new bool[3];
    private bool[] removalInProgress = new bool[3]; // Track if removal is in progress

    private void Start()
    {
        InitializeFuses();
    }

    private void InitializeFuses()
    {
        // Randomly select which fuse is broken
        brokenFuseIndex = Random.Range(0, 3);
        
        // Initialize voltage values for each fuse
        for (int i = 0; i < 3; i++)
        {
            if (i == brokenFuseIndex)
            {
                // Broken fuse has lower voltage
                fuseVoltages[i] = Random.Range(brokenVoltageMin, brokenVoltageMax);
            }
            else
            {
                // Normal fuses have standard voltage
                fuseVoltages[i] = Random.Range(normalVoltageMin, normalVoltageMax);
            }
            
            fuseRemoved[i] = false;
            removalInProgress[i] = false;
            
            // Make sure all fuses have FuseInteractable component
            if (!fuses[i].GetComponent<FuseInteractable>())
            {
                fuses[i].AddComponent<FuseInteractable>();
            }
            
            // Set the fuse index on the interactable component
            fuses[i].GetComponent<FuseInteractable>().fuseIndex = i;
            fuses[i].GetComponent<FuseInteractable>().parentComponent = this;
        }
        
        // Hide the voltage display initially
        if (voltageDisplayText != null)
        {
            voltageDisplayText.gameObject.SetActive(false);
        }
    }

    public void DisplayVoltage(int fuseIndex)
    {
        if (fuseRemoved[fuseIndex])
        {
            voltageDisplayText.gameObject.SetActive(true);
            voltageDisplayText.text = "Click to place a new fuse";
            return;
        }
        
        voltageDisplayText.gameObject.SetActive(true);
        voltageDisplayText.text = $"Voltage: {fuseVoltages[fuseIndex]:F1}V";
    }
    
    public void StartRemoval(int fuseIndex)
    {
        if (fuseRemoved[fuseIndex])
        {
            return;
        }
        
        removalInProgress[fuseIndex] = true;
        voltageDisplayText.gameObject.SetActive(true);
        voltageDisplayText.text = "Starting fuse removal...";
    }
    
    public void CancelRemoval(int fuseIndex)
    {
        if (fuseRemoved[fuseIndex] || !removalInProgress[fuseIndex])
        {
            return;
        }
        
        removalInProgress[fuseIndex] = false;
        voltageDisplayText.gameObject.SetActive(true);
        voltageDisplayText.text = "Fuse removal canceled";
        
        // Optionally hide the display after a short delay
        StartCoroutine(HideDisplayAfterDelay(1.5f));
    }
    
    private IEnumerator HideDisplayAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideDisplay();
    }
    
    public void UpdateRemovalProgress(int fuseIndex, float progress)
    {
        if (fuseRemoved[fuseIndex] || !removalInProgress[fuseIndex])
        {
            return;
        }
        
        voltageDisplayText.gameObject.SetActive(true);
        voltageDisplayText.text = $"Removing fuse... {(progress * 100):F0}%";
    }
    
    public void HideDisplay()
    {
        if (voltageDisplayText != null)
        {
            voltageDisplayText.gameObject.SetActive(false);
        }
    }
    
    public float GetRemovalTime()
    {
        return fuseRemovalTime;
    }
    
    public void RemoveFuse(int fuseIndex)
    {
        if (fuseRemoved[fuseIndex])
        {
            return;
        }
        
        fuseRemoved[fuseIndex] = true;
        removalInProgress[fuseIndex] = false;
        
        // Change the appearance of the fuse to show it's removed
        // This will depend on your visual implementation
        Renderer fuseRenderer = fuses[fuseIndex].GetComponent<Renderer>();
        if (fuseRenderer != null)
        {
            fuseRenderer.enabled = false; // Or change to a "removed" material
        }
        
        // Check if the correct fuse was removed
        if (fuseIndex == brokenFuseIndex)
        {
            // Success! Notify the game system
            voltageDisplayText.text = "Correct fuse removed! Click to place a new one.";
            
            // Here you would call your game's success method
            // GameManager.Instance.FuseComponentSolved();
        }
        else
        {
            // Wrong fuse removed - potential failure
            voltageDisplayText.text = "Wrong fuse removed! Click to place a new one.";
            
            // Here you would call your game's failure or strike method
            // GameManager.Instance.AddStrike();
        }
    }
    
    public void PlaceFuse(int fuseIndex)
    {
        if (!fuseRemoved[fuseIndex])
        {
            return;
        }
        
        // Reset the fuse to working condition
        fuseRemoved[fuseIndex] = false;
        
        // Set a normal voltage for the replaced fuse
        fuseVoltages[fuseIndex] = Random.Range(normalVoltageMin, normalVoltageMax);
        
        // Make the fuse visible again
        Renderer fuseRenderer = fuses[fuseIndex].GetComponent<Renderer>();
        if (fuseRenderer != null)
        {
            fuseRenderer.enabled = true;
        }
        
        // Show success message
        voltageDisplayText.gameObject.SetActive(true);
        voltageDisplayText.text = "New fuse installed successfully!";
        
        // If this was the broken fuse, consider the component fixed
        if (fuseIndex == brokenFuseIndex)
        {
            // Here you would call your game's success method if not already called
            // GameManager.Instance.FuseComponentRepaired();
            
            // Update the broken fuse index to indicate no broken fuses
            brokenFuseIndex = -1;
        }
        
        // Hide the message after a short delay
        StartCoroutine(HideDisplayAfterDelay(2f));
    }
    
    public bool IsFuseRemoved(int fuseIndex)
    {
        return fuseRemoved[fuseIndex];
    }
}