using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CamMover : MonoBehaviour
{
    public Transform targetPoint;
    public float moveDuration = 1.5f;
    public Ease easing = Ease.InOutSine;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isMoved = false;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (isMoved && Input.GetKeyDown(KeyCode.Escape))
        {
            ResetCamera();
        }
    }

    public async void MoveCamera()
    {
        if (targetPoint != null)
        {
            isMoved = true;
            await MoveCamera(targetPoint.rotation, targetPoint.position);
        }
    }

    public async void ResetCamera()
    {
        isMoved = false;
        await MoveCamera(originalRotation, originalPosition);
    }

    private async Task MoveCamera(Quaternion rotation, Vector3 endPosition,
        CancellationToken cancellationToken = default)
    {
        Task moveTask = transform.DOMove(endPosition, moveDuration).SetEase(easing).AsyncWaitForCompletion();
        Task rotateTask = transform.DORotateQuaternion(rotation, moveDuration).SetEase(easing).AsyncWaitForCompletion();
        await Task.WhenAll(moveTask, rotateTask);
    }
}