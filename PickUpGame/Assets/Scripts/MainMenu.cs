using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject OptionsMenu;

    public GameObject MenuFirst, optionsFirst;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MenuFirst);
        StartMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Options()
    {
        StartMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirst);
    }
    public void ShowStartMenu()
    {
        StartMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MenuFirst);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
