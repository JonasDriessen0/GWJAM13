using UnityEngine;
using UnityEngine.UI;  
using TMPro;           

public class NumpadSystem : MonoBehaviour
{
    private int placeInLine;
    private int codeLenght;

    [SerializeField] private string numCode = "";
    [SerializeField] private string codeAttempt = "";

    [SerializeField] private GameObject GuessedCodeLight;
    [SerializeField] private TMP_Text codeDisplay;

    private void Start()
    {
        codeLenght = numCode.Length;
        UpdateDisplay();
    }

    private void CheckCode()
    {
        if (codeAttempt == numCode)
        {
            GuessedCodeLight.SetActive(true);
            Debug.Log(" Correct Code entered");
        }
        else
        {
            Debug.Log("Wrong Code entered");
        }
    }

    public void SetValue(string value)
    {
        if (placeInLine < codeLenght)
        {
            codeAttempt += value;
            placeInLine++;
            UpdateDisplay();
        }

        if (placeInLine == codeLenght)
        {
            CheckCode();
            ResetCode();
        }
    }

    private void ResetCode()
    {
        codeAttempt = "";
        placeInLine = 0;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (codeDisplay != null)
            codeDisplay.text = codeAttempt;
    }
}