using UnityEngine;

public enum ItemWeightClass
{
    Light,
    Medium,
    Heavy
}

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Pickupable : MonoBehaviour
{
    public ItemWeightClass weightClass = ItemWeightClass.Medium;

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
        rb.linearVelocity = Vector3.zero; // Correct property
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        col.enabled = false;

        transform.SetParent(holdParent);
        transform.localPosition = Vector3.zero;
    }

    public void Throw(Vector3 force)
    {
        IsHeld = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(force * ThrowForceMultiplier(), ForceMode.Impulse);
    }

    public float ThrowForceMultiplier()
    {
        return weightClass switch
        {
            ItemWeightClass.Light => 1f,
            ItemWeightClass.Medium => 0.7f,
            ItemWeightClass.Heavy => 0.5f,
            _ => 1f
        };
    }

    public float MovementSpeedMultiplier()
    {
        return weightClass switch
        {
            ItemWeightClass.Light => 1f,
            ItemWeightClass.Medium => 0.85f,
            ItemWeightClass.Heavy => 0.3f,
            _ => 1f
        };
    }
}
