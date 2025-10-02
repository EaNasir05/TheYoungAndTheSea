using UnityEngine;
using UnityEngine.UIElements;

public class SunTravel : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private float duration;
    [SerializeField] private float pivotDistance;
    private float elapsed;
    private bool finished;
    private Vector3 startDirection;
    private Vector3 rotationAxis;

    private void Awake()
    {
        elapsed = 0;
        finished = false;
        startDirection = (transform.position - pivot.position).normalized;
        rotationAxis = Vector3.Cross(startDirection, Vector3.up).normalized;
    }

    private void Update()
    {
        if (!finished)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float angle = Mathf.Lerp(0f, 180f, t);
            Vector3 rotatedDir = Quaternion.AngleAxis(angle, rotationAxis) * startDirection;
            transform.position = pivot.position + rotatedDir * pivotDistance;
            if (t >= 1f)
                finished = true;
        }
    }
}
