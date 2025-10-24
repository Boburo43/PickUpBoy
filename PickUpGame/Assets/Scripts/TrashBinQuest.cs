using UnityEngine;

public class TrashBinQuest : MonoBehaviour
{
    [SerializeField] private GameObject Trash;
    [SerializeField] private Quest TrashBin;
    private void OnTriggerEnter(Collider other)
    {
        QuestManager.Instance.CompleteQuest(TrashBin);
    }
}
