using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public TMP_Text textComponent;
    public float typeSpeed = 0.05f;
    public float fadeDuration = 1f;

    private Coroutine displayCoroutine;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (textComponent == null)
            textComponent = GetComponent<TMP_Text>();
        
        canvasGroup = textComponent.gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        ShowText("This is a test for a text system, this isnt on the miro board right now but i thought it will likely be handy to have for in the future.", 10);
    }

    public void ShowText(string message, float duration)
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayTextRoutine(message, duration));
    }

    private IEnumerator DisplayTextRoutine(string message, float duration)
    {
        textComponent.text = "";
        canvasGroup.alpha = 1;
        
        yield return StartCoroutine(TypewriterEffect(message));
        
        yield return new WaitForSeconds(duration);
        
        yield return StartCoroutine(FadeOutText());
    }

    private IEnumerator TypewriterEffect(string message)
    {
        foreach (char letter in message)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    private IEnumerator FadeOutText()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }
}
