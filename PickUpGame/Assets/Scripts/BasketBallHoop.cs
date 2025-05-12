using UnityEngine;

public class BasketBallHoop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIt");
    }
}
