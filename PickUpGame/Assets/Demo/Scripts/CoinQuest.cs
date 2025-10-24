using UnityEngine;

public class CoinQuest : MonoBehaviour
{

    [SerializeField] private Quest CoinQuestdata;

    private int coinsReq = 4;
    private int coinsThrown = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
            {
                coinsThrown ++;
                Debug.Log("Coin in");

                if(coinsThrown == coinsReq)
                {
                    QuestManager.Instance.CompleteQuest(CoinQuestdata);
                }
            }
    }
}
