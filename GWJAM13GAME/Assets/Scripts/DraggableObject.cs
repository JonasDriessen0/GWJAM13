using UnityEngine;

public class DraggableObject : MonoBehaviour, IClickable
{
    public bool restrictX = false;
    public bool restrictY = false;
    public bool restrictZ = false;
    public Vector3 minLocalPosition = new Vector3(-5, -5, -5);
    public Vector3 maxLocalPosition = new Vector3(5, 5, 5);

    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private Transform parentTransform;

    void Start()
    {
        mainCamera = Camera.main;
        parentTransform = transform.parent ? transform.parent : transform;
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
            Vector3 newPosition = GetMouseWorldPosition() + offset;
            
            if (restrictX) newPosition.x = transform.position.x;
            if (restrictY) newPosition.y = transform.position.y;
            if (restrictZ) newPosition.z = transform.position.z;
            
            Vector3 localPosition = parentTransform.InverseTransformPoint(newPosition);
            localPosition = new Vector3(
                Mathf.Clamp(localPosition.x, minLocalPosition.x, maxLocalPosition.x),
                Mathf.Clamp(localPosition.y, minLocalPosition.y, maxLocalPosition.y),
                Mathf.Clamp(localPosition.z, minLocalPosition.z, maxLocalPosition.z)
            );
            transform.position = parentTransform.TransformPoint(localPosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        
        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return transform.position;
    }
}