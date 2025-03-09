using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7.5f;
    [SerializeField] private Camera playerCamera;
    
    [SerializeField] private float lookSpeed = 2.0f;
    [SerializeField] private float lookXLimit = 45.0f;

    private CharacterController _characterController;
    
    public UnityEvent headBob;
    public UnityEvent stopHeadBob;

    private Vector3 _moveDirection;
    private Vector2 _rotation;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        _characterController = GetComponent<CharacterController>();
        _rotation.y = transform.eulerAngles.y;
    }

    void Update()
    {
        PlayerMove();
    }
    
    private void PlayerMove()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float moveX = movementSpeed * Input.GetAxis("Vertical");
        float moveY = movementSpeed * Input.GetAxis("Horizontal");

        _moveDirection = (forward * moveX) + (right * moveY);
        _characterController.Move(_moveDirection * Time.deltaTime);
        
        PlayerCamera();

        if (_moveDirection.magnitude > 0)
        {
            headBob.Invoke();
        }
    }

    public void PlayerCamera()
    {
        playerCamera.transform.localPosition = new Vector3(0, 0.64f, 0);
        _rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
        _rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
        _rotation.x = Mathf.Clamp(_rotation.x, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(_rotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, _rotation.y);
    }
}