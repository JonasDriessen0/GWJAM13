using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public float startTime = 60f;
    public UnityEvent onTimerComplete;
    public float flashThreshold = 30f;
    public float flashSpeed = 0.5f;

    private float remainingTime;
    private bool isFlashing = false;

    private void Start()
    {
        remainingTime = startTime;
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (remainingTime <= flashThreshold && !isFlashing)
            {
                StartCoroutine(FlashText());
            }

            yield return null;
        }

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