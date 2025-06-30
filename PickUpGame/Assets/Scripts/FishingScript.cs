using UnityEngine;

public class FishingScript : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        UserInputManager.instance.SwitchActionMap("NewMap");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
