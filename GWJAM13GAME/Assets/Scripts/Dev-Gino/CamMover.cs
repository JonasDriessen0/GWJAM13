using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CamMover : MonoBehaviour
{
    public Transform targetPoint;
    
    public float moveDuration = 1.5f; // Smooth transition duration
    public Ease easing = Ease.InOutSine; // Smoother movement

    private UnityEvent camChange;


    public async void MoveCamera()
    {
        await MoveCamera(Quaternion.Euler(45f,0,0f), targetPoint.position);
    }

    private async Task MoveCamera(Quaternion rotation, Vector3 endPosition,
        CancellationToken cancellationToken = default)
    {
        Task moveTask = transform.DOMove(endPosition, moveDuration).SetEase(easing).AsyncWaitForCompletion();
        Task rotateTask = transform.DORotateQuaternion(rotation, moveDuration).SetEase(easing).AsyncWaitForCompletion();
        await Task.WhenAll(moveTask, rotateTask);
    }
}
