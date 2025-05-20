using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class NPCDialogueTrigger : MonoBehaviour
{
    public GameObject textBubbleUI;
    public DialogueData dialogueData;

    private int currentLineIndex = 0;
    private bool playerInRange = false;

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
            ShowCurrentDialogueLine();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (textBubbleUI != null)
                textBubbleUI.SetActive(false);
        }
    }

    private void OnTalkPerformed(InputAction.CallbackContext context)
    {
        if (!playerInRange)
        
            return;

        currentLineIndex = (currentLineIndex + 1) % dialogueData.dialogueLines.Count;
        ShowCurrentDialogueLine();
    }

    private void ShowCurrentDialogueLine()
    {
        if (dialogueData == null || dialogueData.dialogueLines.Count == 0 || textComponent == null)
            return;

        textBubbleUI.SetActive(true);
        textComponent.text = dialogueData.dialogueLines[currentLineIndex];
    }
}
