using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadComponent : MonoBehaviour
{
    private NumpadSystem numPad;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckHitNumpad();
        }
    }
    private void CheckHitNumpad()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            numPad = hit.transform.gameObject.GetComponentInParent<NumpadSystem>();
            if (numPad != null)
            {
                string value = hit.transform.name;
                numPad.SetValue(value);
            }
        }
    }
}
