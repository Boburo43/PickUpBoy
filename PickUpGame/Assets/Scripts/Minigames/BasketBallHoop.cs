using UnityEngine;
using UnityEngine.VFX;
public class BasketBallHoop : MonoBehaviour
{

    [SerializeField] private Transform vfxSpawnPoint;
    [SerializeField] private VisualEffect vfxPrefab;
    [SerializeField] private Quest BasketBallQuest;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIt");
        VisualEffect vfxInstance = Instantiate(vfxPrefab, vfxSpawnPoint.position, vfxSpawnPoint.rotation);
        QuestManager.Instance.CompleteQuest(BasketBallQuest);
        // Optionally, destroy it after a delay to clean up
        Destroy(vfxInstance.gameObject, 2f);
    }
}
