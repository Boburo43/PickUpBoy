using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/NPC Dialogue")]
public class DialogueData : ScriptableObject
{
    [TextArea(2, 5)]
    public List<string> dialogueLines = new List<string>();
}
