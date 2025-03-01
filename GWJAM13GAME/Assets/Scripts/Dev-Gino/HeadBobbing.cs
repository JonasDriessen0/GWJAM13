
using System;
using UnityEngine;
public class HeadBobbing : MonoBehaviour
{
    [SerializeField] private float bobbingSpeed;
    [SerializeField] private float bobbingAmount;
    [SerializeField] private float bobbingSmoothness;
    [SerializeField] private GameObject playerCamera;
    
    public void StartHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount * 1.4f,bobbingSmoothness * Time.deltaTime);
        playerCamera.transform.localPosition += pos;
    }

}