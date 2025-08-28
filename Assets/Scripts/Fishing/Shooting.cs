using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Shooting : MonoBehaviour
{
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

    [Header("Debug")]
    public bool fire;
    public Rigidbody2D testBody;

    [Header("VelocityRightLeft")]
    public float velocityRightLeft;

    [Header("LineWidth")]
    [Range(0.01f, 1.0f)]
    public float lineStartWidth;
    [Range(0.01f, 1.0f)]
    public float lineEndWidth;

    private bool _isShot = true;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        testBody.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    void FixedUpdate()
    {
        if (_isShot)
        {
            DrawTrajectory();
        }
    }

    void DrawTrajectory()
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

    // Return (next position,collision detected)
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


    // Used for debugging.
    void Update()
    {
        if (_isShot)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                launchVelocity.x -= velocityRightLeft;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                _isShot = false;
                testBody.transform.position = transform.position;
                testBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                testBody.linearVelocity = launchVelocity;
            }
        }

    }

}
