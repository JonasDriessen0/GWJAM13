using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    private GameObject lastRightClickedObject = null;
    private float rightClickHoldTime = 0f;
    private bool isHoldingRightClick = false;
    private MouseHoldProgressUI.ProgressHandle currentProgressHandle;

    void Update()
    {
        // Left click detection
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Clicked on: " + hit.collider.gameObject.name);
                
                // Call an action on the clicked object
                IClickable clickable = hit.collider.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.OnClick();
                }
            }
        }

        // Right click detection
        if (Input.GetMouseButtonDown(1)) // Right mouse button press
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Right clicked on: " + hit.collider.gameObject.name);
                
                // Call right-click action on the clicked object
                IRightClickable rightClickable = hit.collider.GetComponent<IRightClickable>();
                if (rightClickable != null)
                {
                    // Get required hold time from the object
                    float holdTime = rightClickable.GetHoldDuration();
                    
                    // Start the progress UI
                    currentProgressHandle = MouseHoldProgressUI.Instance.StartProgress(
                        holdTime,
                        () => rightClickable.OnRightClickComplete(),
                        () => rightClickable.OnRightClickCancel()
                    );
                    
                    rightClickable.OnRightClick();
                    lastRightClickedObject = hit.collider.gameObject;
                    isHoldingRightClick = true;
                    rightClickHoldTime = 0f;
                }
            }
        }

        // Right click hold detection
        if (isHoldingRightClick && Input.GetMouseButton(1)) // Right mouse button hold
        {
            rightClickHoldTime += Time.deltaTime;
            
            if (lastRightClickedObject != null)
            {
                IRightClickable rightClickable = lastRightClickedObject.GetComponent<IRightClickable>();
                if (rightClickable != null)
                {
                    rightClickable.OnRightClickHold(rightClickHoldTime / rightClickable.GetHoldDuration());
                }
            }
        }

        // Right click release detection
        if (isHoldingRightClick && Input.GetMouseButtonUp(1)) // Right mouse button release
        {
            isHoldingRightClick = false;
            
            // Cancel the progress UI
            if (currentProgressHandle != null)
            {
                currentProgressHandle.Cancel();
                currentProgressHandle = null;
            }
            
            if (lastRightClickedObject != null)
            {
                IRightClickable rightClickable = lastRightClickedObject.GetComponent<IRightClickable>();
                if (rightClickable != null)
                {
                    rightClickable.OnRightClickCancel();
                }
                lastRightClickedObject = null;
            }
        }
    }
}