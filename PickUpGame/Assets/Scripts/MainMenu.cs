using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject OptionsMenu;

    private void Awake()
    {
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
    }
    public void ShowStartMenu()
    {
        StartMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
