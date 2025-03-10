using UnityEngine;
using System.Collections;
using TMPro; // Add TextMeshPro namespace

public class SignalMinigame : MonoBehaviour
{
    private GameObject waveVisual;

    public bool hasCompleted;
    
    private OscilloscopeWave playerWave;
    private OscilloscopeWave targetWave;

    private NewDialRotator stretchDial;
    private NewDialRotator timeShiftDial;
    private DraggableObject amplitudeSlider;

    public float matchThreshold = 0.1f; // Allowed error margin
    public int requiredMatches = 3; // Number of successful matches before winning
    private int currentMatches = 0; // Tracks successful matches

    public OscilloscopeWave[] targetWaves; // Array of different target waves to cycle through
    private int currentTargetIndex = 0;

    public AudioSource clearSignalAudio; // Clear signal sound
    public AudioSource staticNoiseAudio; // Static noise sound
    public AudioSource matchBeepAudio; // Beep sound when successfully matching

    private bool isTransitioning = false; // Flag to prevent multiple matches during transition
    public float transitionDelay = 1.5f; // Time in seconds between successful match and next wave
    private bool canMatch = false; // Flag to prevent instant matching after transition

    // For visual feedback
    public GameObject successIndicator; // Optional: Visual feedback for successful match
    
    // Light indicators for each successful match
    public GameObject[] lightIndicators; // Assign 3 lights in the inspector

    // TextMeshPro for displaying status messages
    public TMP_Text statusText; // Assign this in the inspector
    private GameObject statusTextObject; // Reference to the status text's GameObject

    void Start()
    {
        stretchDial = transform.Find("FrequencyDial")?.GetComponent<NewDialRotator>();
        timeShiftDial = transform.Find("PhaseDial")?.GetComponent<NewDialRotator>();
        amplitudeSlider = transform.Find("SliderObj/Slider")?.GetComponent<DraggableObject>();

        waveVisual = GameObject.Find("SignalWaveVisual");
        playerWave = waveVisual.transform.Find("PlayerWave")?.GetComponent<OscilloscopeWave>();

        statusText = waveVisual.transform.Find("Text (TMP)")?.GetComponent<TMP_Text>();
        statusTextObject = statusText.gameObject;

        targetWaves = new OscilloscopeWave[3];
        targetWaves[0] = waveVisual.transform.Find("TargetWave")?.GetComponent<OscilloscopeWave>();
        targetWaves[1] = waveVisual.transform.Find("TargetWave (1)")?.GetComponent<OscilloscopeWave>();
        targetWaves[2] = waveVisual.transform.Find("TargetWave (2)")?.GetComponent<OscilloscopeWave>();

        if (targetWaves.Length > 0 && targetWaves[0] != null)
        {
            targetWave = targetWaves[0]; // Start with the first target wave
            SetActiveTargetWave(0); // Show only the first wave
        }
        else
        {
            Debug.LogError("No target waves assigned in the Inspector!");
        }

        if (successIndicator != null)
        {
            successIndicator.SetActive(false);
        }

        if (lightIndicators != null && lightIndicators.Length > 0)
        {
            foreach (GameObject light in lightIndicators)
            {
                if (light != null)
                {
                    light.SetActive(false);
                }
            }
        }

        if (statusText != null)
        {
            statusText.text = "";
            statusTextObject.SetActive(false);
        }

        // Enable matching immediately instead of using a cooldown
        canMatch = true;
    }

    void Update()
    {
        // Skip processing if components are missing, during transition, or matching is disabled
        if (playerWave == null || targetWave == null || stretchDial == null || 
            timeShiftDial == null || amplitudeSlider == null || isTransitioning || !canMatch)
            return;

        // Get target wave values
        float targetFrequency = targetWave.frequency;
        float targetAmplitude = targetWave.amplitude;
        float targetTimeOffset = targetWave.timeOffset;

        // Apply amplitude as before
        float playerAmplitude = Mathf.Lerp(playerWave.minAmplitude, playerWave.maxAmplitude, amplitudeSlider.Value);
        playerWave.amplitude = playerAmplitude;

        // Use "stretch" dial to scale frequency relative to target
        float stretchFactor = Mathf.Lerp(0.5f, 2f, stretchDial.Value);
        playerWave.frequency = (stretchDial.Value * 10) + 0.001f;

        // Use "time shift" dial to move wave left/right in time
        float timeShift = Mathf.Lerp(0, 1 / playerWave.frequency, timeShiftDial.Value);
        playerWave.timeOffset = timeShift;

        // Calculate differences
        float frequencyDifference = Mathf.Abs(playerWave.frequency - targetFrequency);
        float amplitudeDifference = Mathf.Abs(playerAmplitude - targetAmplitude);
        float timeOffsetDifference = Mathf.Abs(playerWave.timeOffset - targetTimeOffset);

        // Determine match percentage
        float totalDifference = (frequencyDifference + amplitudeDifference + timeOffsetDifference) / 3f;
        float matchPercentage = Mathf.Clamp01(1f - (totalDifference / matchThreshold));

        // Adjust audio volumes
        if (clearSignalAudio && staticNoiseAudio)
        {
            clearSignalAudio.volume = matchPercentage * 0.3f; // Lowered volume
            staticNoiseAudio.volume = (1f - matchPercentage) * 0.3f; // Lowered volume
        }

        // Check if the values are within the matching threshold
        if (totalDifference < matchThreshold)
        {
            OnMatchSuccess();
        }
    }

    void OnMatchSuccess()
    {
        if (isTransitioning || !canMatch) return; // Prevent matching during transition
        
        isTransitioning = true;
        canMatch = false; // Disable matching during transition
        
        Debug.Log("Match success! Transitioning...");
        
        if (matchBeepAudio) matchBeepAudio.Play(); // Play beep sound when matched
        
        // Show success indicator if available
        if (successIndicator != null)
        {
            successIndicator.SetActive(true);
            Debug.Log("Success indicator activated");
        }

        // Update light indicator for current match
        if (lightIndicators != null && currentMatches < lightIndicators.Length)
        {
            if (lightIndicators[currentMatches] != null)
            {
                lightIndicators[currentMatches].SetActive(true);
                Debug.Log("Light indicator " + currentMatches + " activated");
            }
        }

        currentMatches++;
        Debug.Log($"Successful Matches: {currentMatches}/{requiredMatches}");
        
        HideWaveObjects();
        
        statusText.text = "Signal matched, next signal incoming";
        statusTextObject.SetActive(true);
        Debug.Log("Status text activated: " + statusText.text);

        StartCoroutine(TransitionToNextWave());
    }

    IEnumerator TransitionToNextWave()
    {
        yield return new WaitForSeconds(transitionDelay);
        
        if (successIndicator != null)
        {
            successIndicator.SetActive(false);
        }
        
        if (statusText != null && statusTextObject != null)
        {
            statusTextObject.SetActive(false);
        }

        if (currentMatches >= requiredMatches)
        {
            OnWin();
        }
        else
        {
            NextTargetWave();

            ShowWaveObjects();

            // Enable matching immediately instead of using a cooldown
            canMatch = true;
        }

        isTransitioning = false;
    }

    void NextTargetWave()
    {
        currentTargetIndex = (currentTargetIndex + 1) % targetWaves.Length;
        targetWave = targetWaves[currentTargetIndex];
        SetActiveTargetWave(currentTargetIndex);
    }

    void SetActiveTargetWave(int activeIndex)
    {
        for (int i = 0; i < targetWaves.Length; i++)
        {
            if (targetWaves[i] != null)
            {
                targetWaves[i].gameObject.SetActive(i == activeIndex);
            }
        }
    }
    
    void HideWaveObjects()
    {
        if (playerWave != null)
        {
            playerWave.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < targetWaves.Length; i++)
        {
            if (targetWaves[i] != null)
            {
                targetWaves[i].gameObject.SetActive(false);
            }
        }
    }

    void ShowWaveObjects()
    {
        if (playerWave != null)
        {
            playerWave.gameObject.SetActive(true);
        }
        
        SetActiveTargetWave(currentTargetIndex);
    }

    void OnWin()
    {
        if (statusText != null && statusTextObject != null)
        {
            hasCompleted = true;
            statusText.text = "All signals successfully matched!";
            statusTextObject.SetActive(true);
            clearSignalAudio.Stop();
            staticNoiseAudio.Stop();
        }
    }
}