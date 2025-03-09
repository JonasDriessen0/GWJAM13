using UnityEngine;

public class ClickableObject : MonoBehaviour, IClickable
{
    public CamMover camMover; // Reference to the camera movement script
    public Transform moveToTransform; // Target position for the camera

    private void Awake()
    {
        if (camMover == null)
            camMover = FindObjectOfType<CamMover>(); // Automatically find CamMover if not assigned
    }

    public void OnClick()
    {
        Debug.Log(gameObject.name + " was clicked!");
        
        
        // Move camera to assigned transform
        if (camMover != null && moveToTransform != null)
        {
            camMover.targetPoint = moveToTransform;
            camMover.MoveCamera();
        }
    }
}