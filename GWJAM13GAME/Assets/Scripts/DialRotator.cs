using UnityEngine;

public class DialRotator : MonoBehaviour, IClickable
{
    public float rotationSpeed = 1f;
    
    [Header("Rotate Axes")]
    public bool rotateX = false;
    public bool rotateY = true;
    public bool rotateZ = false;
    
    [Header("Rotation options")]
    public bool invertRotation = false;
    public bool lockRotationDirection = false;
    public bool allowClockwise = true;
    public bool allowCounterClockwise = true;

    public float minRotation = -90f;
    public float maxRotation = 90f;
    
    private bool isDragging = false;
    private Vector3 lastMousePosition;

    public float Value { get; private set; } // Normalized value (0 to 1)

    public void OnClick()
    {
        isDragging = true;
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            float rotationAmount = mouseDelta.x * rotationSpeed;

            if (invertRotation)
            {
                rotationAmount = -rotationAmount;
            }

            if (lockRotationDirection)
            {
                bool isClockwise = rotationAmount < 0;
                bool isCounterClockwise = rotationAmount > 0;

                if ((isClockwise && !allowClockwise) || 
                    (isCounterClockwise && !allowCounterClockwise))
                {
                    lastMousePosition = Input.mousePosition;
                    return;
                }
            }

            float rotX = rotateX ? rotationAmount : 0f;
            float rotY = rotateY ? rotationAmount : 0f;
            float rotZ = rotateZ ? rotationAmount : 0f;

            transform.Rotate(rotX, rotY, rotZ);
            lastMousePosition = Input.mousePosition;

            // Normalize the value based on min and max rotation
            float currentRotation = transform.localEulerAngles.y;
            if (currentRotation > 180) currentRotation -= 360; // Convert from 0-360 to -180 to 180
            currentRotation = Mathf.Clamp(currentRotation, minRotation, maxRotation);

            Value = Mathf.InverseLerp(minRotation, maxRotation, currentRotation);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
