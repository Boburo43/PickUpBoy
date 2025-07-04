using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class QuestLogUI : MonoBehaviour
{
    public GameObject questEntryPrefab;
    public Transform questListParent;
    public GameObject questLogPanel;

    private List<string> activeQuests = new List<string>();

    [SerializeField] private GameObject Scroller;

    private void Update()
    {
        // Check for toggle press in UserInputManager
        if (UserInputManager.instance.ToggleQuestList)
        {
            ToggleQuestLog();
        }
    }

    public void ToggleQuestLog()
    {
        if (questLogPanel != null)
        {
            bool isActivating = !questLogPanel.activeSelf;
            questLogPanel.SetActive(isActivating);

            EventSystem.current.SetSelectedGameObject(null);
            UserInputManager.instance.EnableAction("Move");
            if (isActivating)
            {
                EventSystem.current.SetSelectedGameObject(Scroller);
                UserInputManager.instance.DisableAction("Move");
            }
            
        }
    }

    public void AddQuestToLog(string questName)
    {
        if (!activeQuests.Contains(questName))
        {
            activeQuests.Add(questName);

            GameObject entry = Instantiate(questEntryPrefab, questListParent);

            TMP_Text tmpText = entry.GetComponent<TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text = $"• {questName}";
            }
            else
            {
                Debug.LogError("Quest entry prefab is missing a TMP_Text component!");
            }
        }
    }

    public void MarkQuestAsCompleted(string questName)
    {
        foreach (Transform child in questListParent)
        {
            TMP_Text tmpText = child.GetComponent<TMP_Text>();
            if (tmpText != null && tmpText.text == $"• {questName}")
            {
                Transform line = child.Find("CrossedLine");
                if (line != null)
                    line.gameObject.SetActive(true);
            }
        }
    }
}
