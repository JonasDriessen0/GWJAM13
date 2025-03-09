using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A reusable manager for showing a circular progress indicator that follows the mouse cursor
/// when performing hold actions.
/// </summary>
public class MouseHoldProgressUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image circleProgressImage;
    [SerializeField] private RectTransform progressCanvasRect;
    [SerializeField] private Canvas progressCanvas;
    
    [Header("Settings")]
    [SerializeField] private float offsetX = 20f;
    [SerializeField] private float offsetY = 20f;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color failColor = Color.red;
    
    private static MouseHoldProgressUI _instance;
    public static MouseHoldProgressUI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MouseHoldProgressUI>();
                
                if (_instance == null)
                {
                    Debug.LogError("No MouseHoldProgressUI found in the scene. Make sure to add one!");
                }
            }
            return _instance;
        }
    }
    
    private Coroutine activeProgressCoroutine;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        
        // Hide the progress UI initially
        circleProgressImage.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        // Update position to follow mouse cursor when active
        if (circleProgressImage.gameObject.activeInHierarchy)
        {
            UpdateProgressPosition();
        }
    }
    
    /// <summary>
    /// Start showing a circular progress indicator that follows the mouse
    /// and fills up over the specified duration.
    /// </summary>
    /// <param name="duration">Total time in seconds to complete the circle</param>
    /// <param name="onComplete">Action to execute when progress is complete</param>
    /// <param name="onCancel">Action to execute if progress is canceled</param>
    /// <returns>A handle that can be used to cancel the progress</returns>
    public ProgressHandle StartProgress(float duration, Action onComplete = null, Action onCancel = null)
    {
        // Stop any existing progress
        if (activeProgressCoroutine != null)
        {
            StopCoroutine(activeProgressCoroutine);
        }
        
        // Reset the progress circle
        circleProgressImage.fillAmount = 0;
        circleProgressImage.color = defaultColor;
        circleProgressImage.gameObject.SetActive(true);
        
        // Create a new handle for this progress operation
        ProgressHandle handle = new ProgressHandle(this);
        
        // Start the progress coroutine
        activeProgressCoroutine = StartCoroutine(UpdateProgressRoutine(duration, handle, onComplete, onCancel));
        
        return handle;
    }
    
    /// <summary>
    /// Cancel the current progress indicator.
    /// </summary>
    /// <param name="handle">The handle of the progress to cancel</param>
    public void CancelProgress(ProgressHandle handle)
    {
        if (activeProgressCoroutine != null)
        {
            StopCoroutine(activeProgressCoroutine);
            activeProgressCoroutine = null;
        }
        
        circleProgressImage.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Show a success indicator briefly before hiding.
    /// </summary>
    public void ShowSuccess(float displayTime = 0.5f)
    {
        circleProgressImage.fillAmount = 1f;
        circleProgressImage.color = successColor;
        StartCoroutine(HideAfterDelay(displayTime));
    }
    
    /// <summary>
    /// Show a failure indicator briefly before hiding.
    /// </summary>
    public void ShowFailure(float displayTime = 0.5f)
    {
        circleProgressImage.fillAmount = 1f;
        circleProgressImage.color = failColor;
        StartCoroutine(HideAfterDelay(displayTime));
    }
    
    private IEnumerator UpdateProgressRoutine(float duration, ProgressHandle handle, Action onComplete, Action onCancel)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            // If the handle has been canceled, exit
            if (handle.IsCanceled)
            {
                if (onCancel != null)
                {
                    onCancel.Invoke();
                }
                
                circleProgressImage.gameObject.SetActive(false);
                activeProgressCoroutine = null;
                yield break;
            }
            
            // Update progress fill amount
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            circleProgressImage.fillAmount = progress;
            
            yield return null;
        }
        
        // Progress complete
        if (!handle.IsCanceled && onComplete != null)
        {
            onComplete.Invoke();
        }
        
        // Hide immediately or show success briefly
        if (onComplete != null)
        {
            ShowSuccess();
        }
        else
        {
            circleProgressImage.gameObject.SetActive(false);
        }
        
        activeProgressCoroutine = null;
    }
    
    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        circleProgressImage.gameObject.SetActive(false);
    }
    
    private void UpdateProgressPosition()
    {
        // Convert mouse position to canvas position
        Vector2 mousePos = Input.mousePosition;
        
        // Apply offset
        mousePos.x += offsetX;
        mousePos.y += offsetY;
        
        // If using a screen space overlay canvas
        if (progressCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            progressCanvasRect.position = mousePos;
        }
        // If using a screen space camera canvas
        else if (progressCanvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector2 viewportPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                progressCanvas.transform as RectTransform,
                mousePos,
                progressCanvas.worldCamera,
                out viewportPos);
                
            progressCanvasRect.anchoredPosition = viewportPos;
        }
    }
    
    /// <summary>
    /// Handle class used to track and control progress operations
    /// </summary>
    public class ProgressHandle
    {
        private MouseHoldProgressUI owner;
        public bool IsCanceled { get; private set; }
        
        public ProgressHandle(MouseHoldProgressUI owner)
        {
            this.owner = owner;
            IsCanceled = false;
        }
        
        public void Cancel()
        {
            IsCanceled = true;
            owner.CancelProgress(this);
        }
    }
}