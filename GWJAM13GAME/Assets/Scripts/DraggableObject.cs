using UnityEngine;

public class DraggableObject : MonoBehaviour, IClickable
{
    public float minZ = -5f; // Minimum Z position
    public float maxZ = 5f;  // Maximum Z position

    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Transform parentTransform;
    private Vector3 initialLocalPosition; // Stores the original X and Y values

    public float Value { get; private set; } // The slider's value (0 to 1)

    void Start()
    {
        mainCamera = Camera.main;
        parentTransform = transform.parent ? transform.parent : transform;
        initialLocalPosition = parentTransform.InverseTransformPoint(transform.position); // Save initial X and Y
        UpdateValue(); // Set initial value
    }

    public void OnClick()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }

    void Update()
    {
        if (isDragging)
        {
            Cursor.visible = false;

            Vector3 newPosition = GetMouseWorldPosition() + offset;
            Vector3 localPosition = parentTransform.InverseTransformPoint(newPosition);

            // Keep original X and Y, only change Z
            localPosition = new Vector3(initialLocalPosition.x, initialLocalPosition.y, Mathf.Clamp(localPosition.z, minZ, maxZ));
            transform.position = parentTransform.TransformPoint(localPosition);

            UpdateValue();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true;

            isDragging = false;
        }
    }

    private void UpdateValue()
    {
        Vector3 localPosition = parentTransform.InverseTransformPoint(transform.position);

        // Normalize Z value between min and max
        if (localPosition.z <= maxZ && localPosition.z >= minZ)
        {
            Value = Mathf.InverseLerp(minZ, maxZ, localPosition.z);
        }

        Debug.Log($"Slider Value: {Value}");
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position); // Keep movement on a horizontal plane

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return transform.position;
    }
}
