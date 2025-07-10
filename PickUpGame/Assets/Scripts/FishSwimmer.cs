using UnityEngine;

public class FishSwimmer : MonoBehaviour
{
    public BoxCollider swimZoneVolume;
    public float moveSpeed = 2f;
    public float turnSpeed = 2f;
    public float minAngleToMove = 15f;
    public float safeDistanceFromWalls = 0.5f;

    private Rigidbody rb;
    private Vector3 currentTarget;
    private bool isInSwimZone = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;

        PickNewTarget();
    }

    void FixedUpdate()
    {
        if (swimZoneVolume == null) return;

        Bounds bounds = swimZoneVolume.bounds;
        Vector3 pos = transform.position;

        isInSwimZone = bounds.Contains(pos);

        if (!isInSwimZone)
        {
            rb.useGravity = true;
            return; 
        }

        rb.useGravity = false;

        if (Vector3.Distance(pos, currentTarget) < 0.5f)
        {
            PickNewTarget();
            return;
        }

        Vector3 direction = (currentTarget - pos).normalized;
        float angle = Vector3.Angle(transform.forward, direction);

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, lookRotation,
                                                         turnSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothRotation);
        }

        if (angle < minAngleToMove)
        {
            Vector3 newPos = Vector3.MoveTowards(rb.position, currentTarget,
                                                 moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
    }

    void PickNewTarget()
    {
        Bounds bounds = swimZoneVolume.bounds;
        Vector3 min = bounds.min + Vector3.one * safeDistanceFromWalls;
        Vector3 max = bounds.max - Vector3.one * safeDistanceFromWalls;

        currentTarget = new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }

    void OnDrawGizmosSelected()
    {
        if (swimZoneVolume == null) return;

        Bounds bounds = swimZoneVolume.bounds;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Vector3 shrink = Vector3.one * safeDistanceFromWalls * 2f;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size - shrink);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(currentTarget, 0.15f);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, currentTarget);
        }
    }
}
