using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffusingCameraSystem : MonoBehaviour
{
    [SerializeField] private float maxRotation = 60f;
    [SerializeField] private float minRotation = -60f;
    [SerializeField] private float rotationSpeed = 45f;

    private bool isActive = false;
    private float initialRotationY;

    private void Update()
    {
        if (isActive)
        {
            InDiffuseMode();
        }
    }

    public void Activate()
    {
        isActive = true;
        initialRotationY = transform.localEulerAngles.y;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    public void InDiffuseMode()
    {
        float currentY = transform.localEulerAngles.y;

        if (Input.mousePosition.x >= Screen.width - Screen.width / 4)
        {
            if (currentY < maxRotation)
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
        }
        else if (Input.mousePosition.x <= Screen.width - (Screen.width / 4 * 3))
        {
            if (currentY > minRotation)
            {
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }
        }
    }
}
