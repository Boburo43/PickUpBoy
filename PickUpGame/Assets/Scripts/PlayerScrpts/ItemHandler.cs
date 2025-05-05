using UnityEngine;


public class ItemHandler : MonoBehaviour
{
    [SerializeField] private Transform carryPoint;
    [SerializeField] private Transform grabPoint;
    [SerializeField]private LayerMask pickupMask;
    private float pickupRange = 2f;
    private float throwForce = 5f;

    private Pickupable heldItem;
    private InputSystem_Actions controls;


    [SerializeField] LineRenderer lineRenderer;
    private int trajectorySteps = 30;
    private float trajectoryTimeStep = 0.05f;
    private void Awake()
    {
        controls = new InputSystem_Actions();
        controls.Player.PickUp.performed += _ => OnPickUpOrThrow();
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
        Vector3 throwDirection = transform.forward; // or adjust for camera direction if needed
        heldItem.Throw(throwDirection * throwForce);
        heldItem = null;
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
        Vector3 velocity = transform.forward * throwForce;

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
