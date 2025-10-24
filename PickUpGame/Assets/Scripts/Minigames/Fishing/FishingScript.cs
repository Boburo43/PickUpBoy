using UnityEngine;
using UnityEngine.InputSystem;

public class FishingScript : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject fishingPole;
    public void Interact()
    {
        UserInputManager.instance.SwitchActionMap("Fishing");
        fishingPole.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fishingPole.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (UserInputManager.instance.fishQuit)
        {
            UserInputManager.instance.SwitchActionMap("Player");
            fishingPole.SetActive(false);
        }
    }
}
