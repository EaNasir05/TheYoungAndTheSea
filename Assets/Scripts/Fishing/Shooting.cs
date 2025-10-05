using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Shooting : MonoBehaviour
{
    [SerializeField] private AudioClip hookThrowAudio;

    [Header("Input")]
    public Vector2 launchVelocity;

    [Header("Settings")]
    [Range(0.01f, 0.1f)]
    public float timeStep = 0.02f;
    public int maxCalculationSteps = 128;
    [Range(0.01f, 1.0f)]
    public float tolerance;
    public bool showHitMarker;

    [Header("Dependencies")]
    public GameObject hitMarker;
    LineRenderer lineRenderer;

    [Header("State")]
    RaycastHit hit;
    readonly List<Vector3> points = new();

    [Header("Hook")]
    public bool fire;
    public Rigidbody2D _rb;

    [Header("VelocityRightLeft")]
    public float velocityRightLeft;

    [Header("LineWidth")]
    [Range(0.01f, 1.0f)]
    public float lineStartWidth;
    [Range(0.01f, 1.0f)]
    public float lineEndWidth;

    private bool _isShot = true;
    private bool _isMaxCharge = false;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    void FixedUpdate()
    {
        if (_isShot)
        {
            DrawTrajectory();
        }
    }

    void Update()
    {
        if (_isShot)
        {
            if (Input.GetKey(KeyCode.Space) && !_isMaxCharge && !FishingPointsManager.instance.stop)
            {
                if (!lineRenderer.enabled)
                {
                    lineRenderer.enabled = true;
                }
                launchVelocity.x -= velocityRightLeft;
            }

            if ((Input.GetKeyUp(KeyCode.Space) || _isMaxCharge) && !FishingPointsManager.instance.stop)
            {
                Shoot();
                lineRenderer.positionCount = 0;
                lineRenderer.enabled = false;
            }
        }

    }

    void DrawTrajectory()
    {
        if (!_isMaxCharge)
        {
            lineRenderer.startWidth = lineStartWidth;
            lineRenderer.startWidth = lineEndWidth;

            if (!showHitMarker)
            {
                hitMarker.SetActive(false);
            }

            var position = transform.position;
            var velocity = launchVelocity;
            points.Clear();
            points.Add(position);
            while (true)
            {
                bool collision;
                (position, collision) = GetNextPosition(position, velocity);
                points.Add(position);
                if (collision || points.Count >= maxCalculationSteps)
                {
                    break;
                }
                velocity += Physics2D.gravity * timeStep;
            }

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
            lineRenderer.Simplify(tolerance);
        }
    }
    public void Shoot()
    {
        SoundEffectsManager.instance.PlaySFXClip(hookThrowAudio, 1);
        _isShot = false;
        _rb.gravityScale = 1f;
        _rb.transform.position = transform.position;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.linearVelocity = launchVelocity;
    }
    (Vector2, bool) GetNextPosition(Vector2 currectPoint, Vector2 velocity)
    {
        if (Physics.Raycast(currectPoint, velocity, out hit, velocity.magnitude * timeStep))
        {
            if (showHitMarker)
            {
                hitMarker.SetActive(true);
                hitMarker.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(hit.normal));
                hitMarker.transform.Rotate(90, 0, 0);
            }
            return (hit.point, true);
        }
        hitMarker.SetActive(false);
        return (currectPoint + velocity * timeStep, false);
    }
    public void MarkMaxCharge()
    {
        _isMaxCharge = true;
    }

    public void Restart()
    {
        launchVelocity.x = -0.5f;
        showHitMarker = true;
        hitMarker.SetActive(true);
        _isShot = true;
        _isMaxCharge = false;
        points.Clear();
        lineRenderer.positionCount = points.Count;
    }

    public void OnEnable()
    {
        ChargingBar.isMaxCharged += MarkMaxCharge;
        Hook.onFishOutOfWater += Restart;
    }
    public void OnDisable()
    {
        ChargingBar.isMaxCharged -= MarkMaxCharge;
        Hook.onFishOutOfWater -= Restart;
    }
}
