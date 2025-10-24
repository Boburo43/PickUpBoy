using UnityEngine;

public enum QuestState { NotStarted, InProgress, Completed }

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest/New Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    [TextArea] public string description;

    [TextArea] public string reminderLine;
    [TextArea] public string completedLine;

    public QuestState state = QuestState.NotStarted;
}
