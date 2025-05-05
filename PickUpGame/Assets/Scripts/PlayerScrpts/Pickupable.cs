using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Pickupable : MonoBehaviour
{
    public bool IsHeld { get; private set; }
    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void PickUp(Transform holdParent)
    {
        IsHeld = true;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        col.enabled = false;

        transform.SetParent(holdParent);
        transform.localPosition = Vector3.zero;
    }

    public void Throw(Vector3 force)
    {
        IsHeld = false;
        transform.SetParent(null);
        col.enabled = true;
        rb.isKinematic = false;

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);
    }
}
