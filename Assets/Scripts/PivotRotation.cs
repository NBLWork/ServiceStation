using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private float rotationSpeed;

    private Transform localTransform;

    private void Awake()
    {
        localTransform = transform;
    }

    private void Update()
    {
        localTransform.RotateAround(pivot.position, pivot.up, rotationSpeed * Time.deltaTime);
    }
}
