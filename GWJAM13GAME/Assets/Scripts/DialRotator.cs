using UnityEngine;

public class DialRotator : MonoBehaviour, IClickable
{
    public float rotationSpeed = 1f;

    private bool isDragging = false;
    private Vector3 lastMousePosition;

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

            transform.Rotate(0, rotationAmount, 0);
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}