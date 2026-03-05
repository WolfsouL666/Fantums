using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);

    [Header("=== Camera Bounds (ehh temp doe) ===")]
    [SerializeField] private bool useBounds;
    [SerializeField] private float minX = -14f;
    [SerializeField] private float maxX = 21f;
    [SerializeField] private float minY = -2f;
    [SerializeField] private float maxY = 8f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;

        if (useBounds)
        {
            desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);
            desiredPos.y = Mathf.Clamp(desiredPos.y, minY, maxY);
        }

        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }
}