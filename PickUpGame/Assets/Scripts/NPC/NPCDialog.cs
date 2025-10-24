using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider))]
public class NPCDialogueTrigger : MonoBehaviour
{
    public GameObject textBubbleUI;
    [SerializeField] private Transform textAnchor;
    public DialogueData dialogueData;
    public Quest npcQuest;

    public float typingSpeed = 0.05f;
    [SerializeField] private int currentLineIndex = 0;

    private bool playerInRange = false;
    private bool isTyping = false;
    private bool hasShownEndLine = false;

    private string currentLine = "";
    private string lastDisplayedLine = "";

    private Coroutine typingCoroutine;
    private TMPro.TextMeshProUGUI textComponent;

    [SerializeField] private float dialogRadius = 3f;
    private SphereCollider triggerCollider;

    private void Start()
    {
        triggerCollider = GetComponent<SphereCollider>();
        triggerCollider.isTrigger = true;
        triggerCollider.radius = dialogRadius;

        if (textBubbleUI != null)
        {
            textComponent = textBubbleUI.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            textBubbleUI.SetActive(false);
        }
    }

    private void Update()
    {
        // Keep UI anchored
        textBubbleUI.transform.position = textAnchor.position;

        if (!playerInRange)
            return;

        if (UserInputManager.instance.Talk)
        {
            OnTalkPressed();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            textBubbleUI.SetActive(true);

            if (npcQuest != null && npcQuest.state == QuestState.Completed)
            {
                ShowLine(npcQuest.completedLine);
            }
            else if (!string.IsNullOrEmpty(lastDisplayedLine))
            {
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);
                typingCoroutine = StartCoroutine(TypeText(lastDisplayedLine));
            }
            else
            {
                ShowCurrentDialogueLine();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            textBubbleUI.SetActive(false);
            isTyping = false;
        }
    }

    private void OnTalkPressed()
    {
        if (!playerInRange)
            return;

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            textComponent.text = currentLine;
            isTyping = false;
            return;
        }

        HandleDialogueAdvance();
    }

    private void HandleDialogueAdvance()
    {
        if (currentLineIndex + 1 < dialogueData.dialogueLines.Count)
        {
            currentLineIndex++;
            ShowCurrentDialogueLine();
            return;
        }

        if (!hasShownEndLine)
        {
            hasShownEndLine = true;
            ShowLine(dialogueData.endLine);

            if (npcQuest != null && npcQuest.state == QuestState.NotStarted)
            {
                QuestManager.Instance.StartQuest(npcQuest);
            }
            return;
        }

        if (npcQuest != null)
        {
            if (npcQuest.state == QuestState.Completed)
            {
                ShowLine(npcQuest.completedLine);
            }
            else if (npcQuest.state == QuestState.InProgress)
            {
                ShowLine(npcQuest.reminderLine);
            }
        }
    }

    private void ShowCurrentDialogueLine()
    {
        if (dialogueData == null || dialogueData.dialogueLines.Count == 0 || textComponent == null)
            return;

        textBubbleUI.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        currentLine = dialogueData.dialogueLines[currentLineIndex];
        lastDisplayedLine = currentLine;
        typingCoroutine = StartCoroutine(TypeText(currentLine));
    }

    private void ShowLine(string line)
    {
        if (string.IsNullOrEmpty(line))
            return;

        textBubbleUI.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        currentLine = line;
        lastDisplayedLine = line;
        typingCoroutine = StartCoroutine(TypeText(currentLine));
    }

    private IEnumerator TypeText(string line)
    {
        isTyping = true;
        textComponent.text = "";

        foreach (char letter in line)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
