using UnityEngine;

public class FuseInteractable : MonoBehaviour, IClickable, IRightClickable
{
    public int fuseIndex;
    public FuseComponent parentComponent;
    
    // IClickable implementation
    public void OnClick()
    {
        // Check if the fuse is removed
        if (parentComponent.IsFuseRemoved(fuseIndex))
        {
            // If removed, clicking will place a new fuse
            Debug.Log($"Placing new fuse in slot {fuseIndex}");
            parentComponent.PlaceFuse(fuseIndex);
        }
        else
        {
            // If not removed, just display voltage as before
            parentComponent.DisplayVoltage(fuseIndex);
        }
    }
    
    // IRightClickable implementation
    public void OnRightClick()
    {
        // Initialize right-click removal
        Debug.Log($"Starting to remove fuse {fuseIndex}");
        parentComponent.StartRemoval(fuseIndex);
    }
    
    public void OnRightClickHold(float progress)
    {
        // Update the component with progress
        parentComponent.UpdateRemovalProgress(fuseIndex, progress);
    }
    
    public void OnRightClickCancel()
    {
        // Cancel the removal
        Debug.Log($"Canceled removing fuse {fuseIndex}");
        parentComponent.CancelRemoval(fuseIndex);
    }
    
    public void OnRightClickComplete()
    {
        // Removal complete, remove the fuse
        Debug.Log($"Completed removing fuse {fuseIndex}");
        parentComponent.RemoveFuse(fuseIndex);
    }
    
    public float GetHoldDuration()
    {
        // Get the required hold time from the component
        return parentComponent.GetRemovalTime();
    }
}