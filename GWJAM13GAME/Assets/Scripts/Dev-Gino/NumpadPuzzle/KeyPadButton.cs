using UnityEngine;
using UnityEngine.UI;

public class KeypadButton : MonoBehaviour, IClickable
{
    [SerializeField] private string buttonValue;
    private NumpadSystem numPad;

    private void Start()
    {
        numPad = FindObjectOfType<NumpadSystem>();
    }

    public void OnClick()
    {
        if (numPad != null)
        {
            numPad.SetValue(buttonValue);
        }
    }
}
