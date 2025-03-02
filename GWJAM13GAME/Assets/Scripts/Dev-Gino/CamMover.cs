using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CamMover : MonoBehaviour
{
    public Vector3 targetPoint;
    public float moveDuration = 1.5f; // Smooth transition duration
    public Ease easing = Ease.InOutSine; // Smoother movement


    private void Start()
    {
        transform.DORotate(new Vector3(45, -45, 0), 5f);

    }

    public void ToggleCameraPosition()
    {
        
    }

    private void MoveCamera(Vector3 newPosition, Quaternion newRotation)
    {
        transform.DOMove(newPosition, moveDuration).SetEase(easing);
        transform.DORotate(new Vector3(0, -25, 0), 5f);
    }
}
