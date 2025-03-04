using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusingCameraSystem : MonoBehaviour
{
    public bool inDiffuse;

    public float maxRotation = 235;
    public float minRotation = 125;

    public float rotationSpeed = 45f;

    private void Update() 
    {
        if (inDiffuse)
        {
            InDiffuseMode();
        }
        else
        {
            Debug.Log("try again later");
        }
    }
    
    private void InDiffuseMode()
    {
        if (Input.mousePosition.x >= Screen.width - Screen.width / 4)
        {
            if (this.transform.localEulerAngles.y < maxRotation)
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
        }
        else if (Input.mousePosition.x <= Screen.width - (Screen.width / 4 * 3))
        {
            if (this.transform.localEulerAngles.y > minRotation)
            {
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }
        }
    }
}
