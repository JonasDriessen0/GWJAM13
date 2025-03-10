using System;
using System.Collections;
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
    [SerializeField] private GameObject multitool; // Reference to the multitool object

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fuseRemoveSFX;
    [SerializeField] private AudioClip fuseInsertSFX;
    
    private int brokenFuseIndex; // Which fuse is broken
    private float[] fuseVoltages = new float[3];
    private bool[] fuseRemoved = new bool[3];
    private bool[] removalInProgress = new bool[3]; // Track if removal is in progress
    private Coroutine hideMultitoolCoroutine;

    public bool hasCompleted;
    
    private void Start()
    {
        InitializeFuses();

        voltageDisplayText = Camera.main.transform.Find("Multytool Devise/text")?.GetComponent<TMP_Text>();

        if (multitool == null)
        {
            multitool = GameObject.Find("Multytool Devise");
        }

        if (multitool != null)
        {
            multitool.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Multitool object not found!");
        }

        // Ensure we have an AudioSource component
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void InitializeFuses()
    {
        // Reset completion status when initializing
        hasCompleted = false;
        
        // Randomly select which fuse is broken
        brokenFuseIndex = Random.Range(0, 3);

        // Initialize voltage values for each fuse
        for (int i = 0; i < 3; i++)
        {
            fuseVoltages[i] = (i == brokenFuseIndex) 
                ? Random.Range(brokenVoltageMin, brokenVoltageMax) // Broken fuse voltage
                : Random.Range(normalVoltageMin, normalVoltageMax); // Normal fuse voltage

            fuseRemoved[i] = false;
            removalInProgress[i] = false;

            if (!fuses[i].GetComponent<FuseInteractable>())
            {
                fuses[i].AddComponent<FuseInteractable>();
            }

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
            ShowMultitool();
            voltageDisplayText.text = "Click to place a new fuse";
            return;
        }

        ShowMultitool();
        voltageDisplayText.text = $"Voltage: {fuseVoltages[fuseIndex]:F1}V";
    }

    private void ShowMultitool()
    {
        if (multitool != null)
        {
            multitool.SetActive(true);

            // Reset any ongoing hiding coroutine
            if (hideMultitoolCoroutine != null)
            {
                StopCoroutine(hideMultitoolCoroutine);
            }

            // Start a new coroutine to hide it after 3 seconds
            hideMultitoolCoroutine = StartCoroutine(HideMultitoolAfterDelay(3f));
        }
    }

    private IEnumerator HideMultitoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (multitool != null)
        {
            multitool.SetActive(false);
        }
    }

    public void StartRemoval(int fuseIndex)
    {
        if (fuseRemoved[fuseIndex])
        {
            return;
        }

        removalInProgress[fuseIndex] = true;
        ShowMultitool();
        voltageDisplayText.text = "Starting fuse removal...";
    }

    public void CancelRemoval(int fuseIndex)
    {
        if (fuseRemoved[fuseIndex] || !removalInProgress[fuseIndex])
        {
            return;
        }

        removalInProgress[fuseIndex] = false;
        ShowMultitool();
        voltageDisplayText.text = "Fuse removal canceled";

        StartCoroutine(HideMultitoolAfterDelay(1.5f));
    }

    public void UpdateRemovalProgress(int fuseIndex, float progress)
    {
        if (fuseRemoved[fuseIndex] || !removalInProgress[fuseIndex])
        {
            return;
        }

        ShowMultitool();
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

        Renderer fuseRenderer = fuses[fuseIndex].GetComponent<Renderer>();
        if (fuseRenderer != null)
        {
            fuseRenderer.enabled = false;
        }

        if (fuseRemoveSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(fuseRemoveSFX);
        }
        
        if (fuseIndex == brokenFuseIndex)
        {
            voltageDisplayText.text = "Correct fuse removed! Click to place a new one.";
            brokenFuseIndex = -1;
        }
        else
        {
            voltageDisplayText.text = "Wrong fuse removed! Click to place a new one.";
        }
    }

    public void PlaceFuse(int fuseIndex)
    {
        if (!fuseRemoved[fuseIndex])
        {
            return;
        }

        fuseRemoved[fuseIndex] = false;
        fuseVoltages[fuseIndex] = Random.Range(normalVoltageMin, normalVoltageMax);

        Renderer fuseRenderer = fuses[fuseIndex].GetComponent<Renderer>();
        if (fuseRenderer != null)
        {
            fuseRenderer.enabled = true;
        }

        if (fuseRemoveSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(fuseRemoveSFX);
        }
        
        ShowMultitool();
        voltageDisplayText.text = "New fuse installed successfully!";
        hasCompleted = true;

        // Check if this was the previously broken fuse position
        if (brokenFuseIndex == -1 && fuseIndex == brokenFuseIndex)
        {
            // Mark the task as completed when the broken fuse is replaced
            hasCompleted = true;
            voltageDisplayText.text = "New fuse installed successfully! Task completed!";
        }

        StartCoroutine(HideMultitoolAfterDelay(2f));
    }

    public bool IsFuseRemoved(int fuseIndex)
    {
        return fuseRemoved[fuseIndex];
    }
}