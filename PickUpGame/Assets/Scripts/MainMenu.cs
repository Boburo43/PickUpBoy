using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject StartCanvas;
    [SerializeField] private GameObject OptionsCanvas;
    [SerializeField] private GameObject ControllerCanvas;
    [SerializeField] private GameObject KeyBoardCanvas;
    [SerializeField] private GameObject AudioCanvas;

    [SerializeField] private GameObject menuFirst, settingsFirst, controllerFirst, keyboardFirst, audioFirst;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirst);
        StartCanvas.SetActive(true);
        OptionsCanvas.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Options()
    {
        StartCanvas.SetActive(false);
        OptionsCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirst);
    }
    public void ShowStartMenu()
    {
        StartCanvas.SetActive(true);
        OptionsCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuFirst);
    }

    public void BackToSettings()
    {
        OptionsCanvas.SetActive(true);

        ControllerCanvas.SetActive(false);
        KeyBoardCanvas.SetActive(false);
        AudioCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirst);

    }

    public void GamePad()
    {
        ControllerCanvas.SetActive(true);
        OptionsCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controllerFirst);
    }

    public void Keyboard()
    {
        KeyBoardCanvas.SetActive(true);
        OptionsCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(keyboardFirst);
    }

    public void Audio()
    {
        AudioCanvas.SetActive(true);
        OptionsCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(audioFirst);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
