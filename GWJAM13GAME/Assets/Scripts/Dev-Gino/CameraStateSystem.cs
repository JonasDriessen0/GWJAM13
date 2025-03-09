using UnityEngine;
using UnityEngine.Events;

public enum CameraStates { Idle, Diffusing }

public class CameraStateSystem : MonoBehaviour
{
    private CameraStates _state = CameraStates.Idle;
    [SerializeField] private bool isDiffusing;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private DiffusingCameraSystem diffusingCamera;

    [SerializeField] private UnityEvent diffuseMode;
    [SerializeField] private UnityEvent playermode;
    [SerializeField] private UnityEvent cameraTransition;

    private void Start()
    {
        UpdateCameraState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleState();
        }
    }

    public void ToggleState()
    {
        isDiffusing = !isDiffusing;
        _state = isDiffusing ? CameraStates.Diffusing : CameraStates.Idle;
        UpdateCameraState();
    }

    private void UpdateCameraState()
    {
        switch (_state)
        {
            case CameraStates.Idle:
                playerController.enabled = true;
                diffusingCamera.enabled = false;
                diffusingCamera.Deactivate();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                playermode?.Invoke();
                break;

            case CameraStates.Diffusing:
                cameraTransition?.Invoke();
                playerController.enabled = false;
                diffusingCamera.enabled = true;
                diffusingCamera.Activate();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                diffuseMode?.Invoke();
                break;
        }
    }
}