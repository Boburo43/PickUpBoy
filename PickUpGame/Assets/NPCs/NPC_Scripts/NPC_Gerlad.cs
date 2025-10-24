using UnityEngine;
using UnityEngine.VFX;

public class NPC_Gerlad : MonoBehaviour
{
    [SerializeField] private Quest GeraldQuest;
    [SerializeField] private GameObject Gerald;
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
        if(other.gameObject == Gerald)
        {
            QuestManager.Instance.CompleteQuest(GeraldQuest);

        }
    }
}
