using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public float startTime = 60f;
    public UnityEvent onTimerComplete;
    public float flashThreshold = 30f;
    public float flashSpeed = 0.3f; // Increased flash speed

    public Volume postProcessingVolume;
    public float vignetteStart = 0.2f;
    public float vignetteEnd = 0.6f;
    public float chromaticAberrationStart = 0f;
    public float chromaticAberrationEnd = 1f;

    private float remainingTime;
    private bool isFlashing = false;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;

    private void Start()
    {
        remainingTime = startTime;
        
        if (postProcessingVolume.profile.TryGet(out vignette) &&
            postProcessingVolume.profile.TryGet(out chromaticAberration))
        {
            vignette.intensity.value = vignetteStart;
            chromaticAberration.intensity.value = chromaticAberrationStart;
        }
        
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();
            UpdateEffects();

            if (remainingTime <= flashThreshold && !isFlashing)
            {
                StartCoroutine(FlashText());
            }

            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        remainingTime = 0;
        UpdateTimerDisplay();
        onTimerComplete?.Invoke();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateEffects()
    {
        float t = 1f - (remainingTime / startTime);
        
        if (vignette != null)
            vignette.intensity.value = Mathf.Lerp(vignetteStart, vignetteEnd, t);
        
        if (chromaticAberration != null)
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberrationStart, chromaticAberrationEnd, t);
    }

    private IEnumerator FlashText()
    {
        isFlashing = true;
        while (remainingTime > 0)
        {
            timerText.enabled = !timerText.enabled;
            yield return new WaitForSeconds(flashSpeed);
        }
        timerText.enabled = true;
        isFlashing = false;
    }
}
