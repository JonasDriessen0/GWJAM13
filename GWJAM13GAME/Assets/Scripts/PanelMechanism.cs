using UnityEngine;

public class PanelMechanism : MonoBehaviour
{
    public ScrewMechanism[] screws;
    private int unscrewedCount = 0;

    public void OnScrewRemoved()
    {
        unscrewedCount++;

        if (unscrewedCount >= screws.Length)
        {
            OpenPanel();
        }
    }

    private void OpenPanel()
    {
        Debug.Log("All screws removed. Opening panel.");
        // Add animation or logic to remove/move panel
        gameObject.SetActive(false);
    }
}