using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Windows;


public class ItemHandler : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    [SerializeField] private Transform grabPoint;
    [SerializeField]private LayerMask pickupMask;
    private float pickupRange = 2f;
    private float throwForce = 5f;
    private float throwAngle = 30f;

    private float minThrowForce = 2f;
    private float maxThrowForce = 15f;

    private float minThrowAngle = 15f;
    private float maxThrowAngle = 80f;

    private float angleAdjustSpeed = 60f;
    private float forceAdjustSpeed = 20f;

    private Pickupable heldItem;
    private InputSystem_Actions controls;
    private float angleInput;
    private float forceInput;

    [SerializeField] LineRenderer lineRenderer;
    private int trajectorySteps = 50;
    private float trajectoryTimeStep = 0.05f;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        controls.Player.PickUp.performed += _ => OnPickUpOrThrow();
        controls.Player.AdjustAngle.performed += ctx => angleInput = ctx.ReadValue<float>();
        controls.Player.AdjustAngle.canceled += _ => angleInput = 0f;

        controls.Player.AdjustForce.performed += ctx => forceInput = ctx.ReadValue<float>();
        controls.Player.AdjustForce.canceled += _ => forceInput = 0f;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        if (heldItem != null)
        {
            ShowThrowArc();
        }
        else
        {
            lineRenderer.positionCount = 0; // hide arc
        }
        // Draw a wireframe sphere in the Scene view (editor only)
        DebugDrawSphere(grabPoint.position, pickupRange, Color.green);

        throwAngle += angleInput * angleAdjustSpeed * Time.deltaTime;
        throwForce += forceInput * forceAdjustSpeed * Time.deltaTime;

        throwAngle = Mathf.Clamp(throwAngle, minThrowAngle, maxThrowAngle);
        throwForce = Mathf.Clamp(throwForce, minThrowForce, maxThrowForce);
    }

    void OnPickUpOrThrow()
    {
        if (heldItem == null)
        {
            TryPickup();
        }
        else
        {
            ThrowItem();
        }
    }

    void TryPickup()
    {
        Collider[] hits = Physics.OverlapSphere(grabPoint.position, pickupRange, pickupMask);
        foreach (var hit in hits)
        {
            Pickupable pickup = hit.GetComponent<Pickupable>();
            if (pickup != null && !pickup.IsHeld)
            {
                heldItem = pickup;
                heldItem.PickUp(carryPoint);
                break;
            }
        }
    }

    void ThrowItem()
    {
        Vector3 forward = transform.forward;
        Vector3 up = Vector3.up;
        Quaternion angleRotation = Quaternion.AngleAxis(throwAngle, Vector3.Cross(forward, up));
        Vector3 throwDir = angleRotation * forward;

        heldItem.Throw(throwDir * throwForce * heldItem.ThrowForceMultiplier());
        heldItem = null;
    }
    public float GetHeldItemSpeedMultiplier()
    {
        return heldItem != null ? heldItem.MovementSpeedMultiplier() : 1f;
    }

    void DebugDrawSphere(Vector3 center, float radius, Color color, int segments = 24)
    {
        float step = 360f / segments;
        for (int i = 0; i < segments; i++)
        {
            float angleA = i * step * Mathf.Deg2Rad;
            float angleB = (i + 1) * step * Mathf.Deg2Rad;

            Vector3 offsetA = new Vector3(Mathf.Cos(angleA), 0, Mathf.Sin(angleA)) * radius;
            Vector3 offsetB = new Vector3(Mathf.Cos(angleB), 0, Mathf.Sin(angleB)) * radius;

            // Draw horizontal circle
            Debug.DrawLine(center + offsetA, center + offsetB, color);

            // Draw vertical circle
            Vector3 yOffsetA = new Vector3(0, Mathf.Cos(angleA), Mathf.Sin(angleA)) * radius;
            Vector3 yOffsetB = new Vector3(0, Mathf.Cos(angleB), Mathf.Sin(angleB)) * radius;
            Debug.DrawLine(center + yOffsetA, center + yOffsetB, color);

            // Draw cross circle
            Vector3 xzOffsetA = new Vector3(Mathf.Cos(angleA), Mathf.Sin(angleA), 0) * radius;
            Vector3 xzOffsetB = new Vector3(Mathf.Cos(angleB), Mathf.Sin(angleB), 0) * radius;
            Debug.DrawLine(center + xzOffsetA, center + xzOffsetB, color);
        }
    }
    void ShowThrowArc()
    {
        Vector3[] points = new Vector3[trajectorySteps];

        Vector3 startPos = carryPoint.position;
        Quaternion angleRot = Quaternion.AngleAxis(throwAngle, Vector3.Cross(transform.forward, Vector3.up));
        Vector3 throwDir = angleRot * transform.forward * heldItem.ThrowForceMultiplier();
        Vector3 velocity = throwDir * throwForce * heldItem.ThrowForceMultiplier();


        for (int i = 0; i < trajectorySteps; i++)
        {
            float t = i * trajectoryTimeStep;
            Vector3 point = startPos + velocity * t + 0.5f * Physics.gravity * t * t;
            points[i] = point;
        }

        lineRenderer.positionCount = trajectorySteps;
        lineRenderer.SetPositions(points);
    }
}
