using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumpadSystem : MonoBehaviour
{
    private int placeInLine;
    private int codeLenght;

    [SerializeField] private string numCode = "";
    [SerializeField] private string codeAttempt;

    [SerializeField] private GameObject GuessedCodeLight;

    private void Start()
    {
        codeLenght = numCode.Length;
    }

    private void CheckCode()
    {
        if (codeAttempt == numCode)
        {
            GuessedCodeLight.SetActive(true);
        }
    }

    public void SetValue(string value)
    {
        if (placeInLine < codeLenght)
        {
            codeAttempt += value;
            placeInLine++;
        }

        if (placeInLine == codeLenght)
        {
            CheckCode();
            codeAttempt = "";
            placeInLine = 0;
        }
    }
}