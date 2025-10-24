using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogueTrigger : MonoBehaviour
{
    public GameObject textBubbleUI;
    public DialogueData dialogueData;
    public Quest npcQuest; // Optional quest reference

    public float typingSpeed = 0.05f;
    [SerializeField] private float inputCooldown = 0.2f;

    [SerializeField]private int currentLineIndex = 0;
    private bool playerInRange = false;
    private bool isTyping = false;
    private bool inputLocked = false;
    private bool hasShownEndLine = false;

    private string currentLine = "";
    private string lastDisplayedLine = "";


    private Coroutine typingCoroutine;
    private TMPro.TextMeshProUGUI textComponent;
    private InputSystem_Actions controls;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        controls.Player.Talk.performed += OnTalkPerformed;
    }

    private void OnEnable() => controls?.Enable();
    private void OnDisable() => controls?.Disable();

    private void Start()
    {
        if (textBubbleUI != null)
        {
            textComponent = textBubbleUI.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            textBubbleUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            textBubbleUI.SetActive(true);
            if(npcQuest != null)
            {
                if(npcQuest.state == QuestState.Completed)
                {
                    ShowLine(npcQuest.completedLine);
                }
            }
            if (!string.IsNullOrEmpty(lastDisplayedLine))
            {
                // Start typing the last displayed line again
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

    private void OnTalkPerformed(InputAction.CallbackContext context)
    {
        if (!playerInRange || inputLocked)
            return;

        if (isTyping)
        {
            // Finish line immediately
            StopCoroutine(typingCoroutine);
            textComponent.text = currentLine;
            isTyping = false;
            return; 
        }

        HandleDialogueAdvance();
    }

    private void HandleDialogueAdvance()
    {
        // Case 1: Still going through regular dialogue
        if (currentLineIndex + 1 < dialogueData.dialogueLines.Count)
        {
            currentLineIndex++;
            ShowCurrentDialogueLine();
            return;
        }

        // Case 2: Finished regular dialogue, show endLine once
        if (!hasShownEndLine)
        {
            hasShownEndLine = true;

            ShowLine(dialogueData.endLine);

            // Start quest if it's not started yet
            if (npcQuest != null && npcQuest.state == QuestState.NotStarted)
            {
                QuestManager.Instance.StartQuest(npcQuest);
            }
            return;
        }

        // Case 3: After endLine has been shown, handle quest-specific lines
        if (npcQuest != null)
        {
            if (npcQuest.state == QuestState.Completed)
            {
                ShowLine(npcQuest.completedLine);
                return;
            }
            if (npcQuest.state == QuestState.InProgress)
            {
                ShowLine(npcQuest.reminderLine);
                return;
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
