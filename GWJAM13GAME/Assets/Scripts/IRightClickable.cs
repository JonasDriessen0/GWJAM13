public interface IRightClickable
{
    // Called when right click is initiated
    void OnRightClick();
    
    // Called during right click hold, with progress percentage (0-1)
    void OnRightClickHold(float progress);
    
    // Called when right click is released before completion
    void OnRightClickCancel();
    
    // Called when right click hold is complete
    void OnRightClickComplete();
    
    // Returns the time in seconds needed to hold for completion
    float GetHoldDuration();
}