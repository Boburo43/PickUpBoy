using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class NPCDialogueTrigger : MonoBehaviour
{
    public GameObject textBubbleUI;
    public DialogueData dialogueData;

    private int currentLineIndex = 0;
    private bool playerInRange = false;
    private bool isTyping = false;
    private string currentLine = "";

    [SerializeField]private bool dialogueFinished = false;

    private Coroutine typingCoroutine;
    public float typingSpeed = 0.05f;

    [SerializeField] private float inputCooldown = 0.2f;
    private bool inputLocked = false;

    private TMPro.TextMeshProUGUI textComponent;
    private InputSystem_Actions controls;

    private void Awake()
    {
        controls = new InputSystem_Actions();
        controls.Player.Talk.performed += OnTalkPerformed;
    }


    private void OnEnable()
    {
        controls?.Enable();
    }

    private void OnDisable()
    {
        controls?.Disable();
    }

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
            if(dialogueFinished)
            {
                ShowReminderLine();
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
        if (!playerInRange || dialogueData == null)
            return;

        StartCoroutine(InputCooldown());

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            textComponent.text = currentLine;
            isTyping = false;
        }
        else
        {
            if (dialogueFinished)
            {
                ShowReminderLine();
                return;
            }

            // Advance to the next line *only* if not at the last one
            if (currentLineIndex + 1 >= dialogueData.dialogueLines.Count)
            {
                dialogueFinished = true;
                ShowReminderLine();
            }
            else
            {
                currentLineIndex++;
                ShowCurrentDialogueLine();
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
        typingCoroutine = StartCoroutine(TypeText(currentLine));
    }

    private void ShowReminderLine()
    {
        dialogueFinished = true;

        if (string.IsNullOrEmpty(dialogueData.reminderLine))
            return;

        textBubbleUI.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        currentLine = dialogueData.reminderLine;
        typingCoroutine = StartCoroutine(TypeText(currentLine));
    }



    private IEnumerator TypeText(string line)
    {
        isTyping = true;
        textComponent.text = "";

        foreach (char letter in line.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
    private IEnumerator InputCooldown()
    {
        inputLocked = true;
        yield return new WaitForSeconds(inputCooldown);
        inputLocked = false;
    }


}
