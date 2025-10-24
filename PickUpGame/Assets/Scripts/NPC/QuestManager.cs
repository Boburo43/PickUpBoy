using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public QuestLogUI questLogUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

       
    }

    public void StartQuest(Quest quest)
    {
        if (quest != null && quest.state == QuestState.NotStarted)
        {
            quest.state = QuestState.InProgress;
            Debug.Log($"Quest Started: {quest.questName}");

            if (questLogUI != null)
                questLogUI.AddQuestToLog(quest.questName);
        }
    }

    public void CompleteQuest(Quest quest)
    {
        if (quest != null && quest.state == QuestState.InProgress)
        {
            quest.state = QuestState.Completed;
            Debug.Log($"Quest Completed: {quest.questName}");

            questLogUI.MarkQuestAsCompleted(quest.questName);
        }
    }
}
