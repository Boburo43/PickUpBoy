using UnityEngine;
using UnityEngine.VFX;
public class BasketBallHoop : MonoBehaviour
{

    [SerializeField] private Transform vfxSpawnPoint;
    [SerializeField] private VisualEffect vfxPrefab;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIt");
        VisualEffect vfxInstance = Instantiate(vfxPrefab, vfxSpawnPoint.position, vfxSpawnPoint.rotation);

        // Optionally, destroy it after a delay to clean up
        Destroy(vfxInstance.gameObject, 2f);
    }
}
