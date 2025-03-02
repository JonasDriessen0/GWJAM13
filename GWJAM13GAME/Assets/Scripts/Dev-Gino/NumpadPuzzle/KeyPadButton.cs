using UnityEngine;
using UnityEngine.UI;

public class KeypadButton : MonoBehaviour, IClickable
{
    [SerializeField] private string buttonValue;  // Set this in the Inspector
    private NumpadSystem numPad;

    private void Start()
    {
        numPad = FindObjectOfType<NumpadSystem>(); // Find the NumpadSystem in the scene
    }

    public void OnClick()
    {
        if (numPad != null)
        {
            numPad.SetValue(buttonValue);
        }
    }
}
