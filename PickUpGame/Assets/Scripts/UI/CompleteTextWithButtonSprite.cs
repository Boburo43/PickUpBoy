using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public static class CompleteTextWithButtonSprite
{
    public static string ReadAndReplaceBinding(string textToDisplay, InputBinding actionNeeded, TMPro.TMP_SpriteAsset spriteAsset)
    {
        string stringButtonName = actionNeeded.ToString();
        stringButtonName = RenameInput(stringButtonName);

        textToDisplay = textToDisplay.Replace(
            "BUTTONPROMT",
            $"<sprite=\"{spriteAsset.name}\" name=\"{stringButtonName}\">");

        return textToDisplay;
    }

    private static string RenameInput(string stringButtonName)
    {
        stringButtonName = stringButtonName.Replace(
            "Player:", String.Empty);

        stringButtonName = stringButtonName.Replace("<Gamepad>/", "Gamepad_");

        stringButtonName = stringButtonName.Replace("<Keyboard>/", "Keyboard_");

        return stringButtonName;
    }
}
