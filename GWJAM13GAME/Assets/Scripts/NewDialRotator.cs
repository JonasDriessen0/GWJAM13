using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class NewDialRotator : MonoBehaviour, IClickable
{
    public float rotationSpeed = 5f;

    [Header("Rotate Axis (Only one should be true)")]
    public bool rotateX = false;
    public bool rotateY = true;  // Default to Y rotation
    public bool rotateZ = false;

    [Header("Rotation Options")]
    public bool invertRotation = false;

    private bool isDragging = false;
    private Vector3 lastMousePosition;
    private float totalRotation = -90f; // Start with -90° offset

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
            Cursor.visible = false;
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            float rotationAmount = mouseDelta.x * rotationSpeed * Time.deltaTime;

            if (invertRotation)
            {
                rotationAmount = -rotationAmount;
            }

            // Determine rotation axis (only Y axis for now)
            Vector3 rotationAxis = rotateX ? Vector3.right : (rotateY ? Vector3.up : Vector3.forward);

            // Calculate the new total rotation
            totalRotation += rotationAmount;

            // Clamp total rotation between -90° and 90° (0° to 180° relative to the offset)
            totalRotation = Mathf.Clamp(totalRotation, -90f, 90f);

            // Apply clamped rotation to the dial
            Quaternion targetRotation = Quaternion.Euler(
                rotateX ? totalRotation : transform.localEulerAngles.x,
                rotateY ? totalRotation : transform.localEulerAngles.y,
                rotateZ ? totalRotation : transform.localEulerAngles.z
            );
            transform.localRotation = targetRotation;

            // Normalize the value (0 to 1) based on rotation
            Value = (totalRotation + 90f) / 180f; // Adjust for -90° offset
            lastMousePosition = Input.mousePosition;

            Debug.Log($"Dial Value: {Value}");
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true;
            isDragging = false;
        }
    }
}