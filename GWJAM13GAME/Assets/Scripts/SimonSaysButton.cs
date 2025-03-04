using UnityEngine;
using System;
using System.Collections;

public class SimonSaysButton : MonoBehaviour, IClickable
{
    public event Action<SimonSaysButton> OnButtonPressed;
    private Renderer buttonRenderer;
    private Color originalColor;

    private void Start()
    {
        buttonRenderer = GetComponent<Renderer>();
        originalColor = buttonRenderer.material.color;
    }

    public void OnClick()
    {
        OnButtonPressed?.Invoke(this);
        //StartCoroutine(FlashWhite());
    }

    public void Flash()
    {
        StartCoroutine(FlashWhite());
    }

    public void FlashRed()
    {
        StartCoroutine(FlashFailure());
    }

    private IEnumerator FlashWhite()
    {
        buttonRenderer.material.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        buttonRenderer.material.color = originalColor;
    }

    private IEnumerator FlashFailure()
    {
        buttonRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        buttonRenderer.material.color = originalColor;
    }
}