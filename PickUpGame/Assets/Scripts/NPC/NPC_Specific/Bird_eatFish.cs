using UnityEngine;

public class Bird_eatFish: MonoBehaviour
{

    [SerializeField] private GameObject[] fishes;
    [SerializeField] private Collider EatCollider;

    [SerializeField] private Quest quest;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject fish in fishes)
        {
            if (other.gameObject == fish)
            {
                Debug.Log("you ate " + fish.name);
                FishEaten(fish);
            }
        }
    }

    void FishEaten(GameObject fish)
    {
        fish.SetActive(false);
        QuestManager.Instance.CompleteQuest(quest);
    }
}
