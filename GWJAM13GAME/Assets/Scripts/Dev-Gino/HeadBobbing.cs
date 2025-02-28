
using UnityEngine;
public class HeadBobbing : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed;
    [SerializeField] private float bobHeight;
    [SerializeField] private GameObject playerCamera;

    private Vector3 vel;
    
    private Animator headBobAni;
    private PlayerController _playerController;

    // Update is called once per frame
    private void Start()
    {
        headBobAni = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        float speed = vel.magnitude;
        
        float sin = bobHeight * Mathf.Sin(bobbingSpeed * speed);
        playerCamera.transform.position = Vector3.up * sin; 

    }
}
