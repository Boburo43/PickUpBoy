using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputManager : MonoBehaviour
{
   public static UserInputManager instance;

    public Vector2 MoveInput {  get; private set; }

    public float AdjustAngle { get; private set; }
    public float AdjustForce { get; private set; }
    public bool PickUp { get; private set; }
    public bool Talk {  get; private set; }
    public bool ToggleQuestList { get; private set; }
    public bool Store {  get; private set; }
    public bool fishQuit { get; private set; }


    private PlayerInput _playerInput;

    [SerializeField]private string currentActionMap = "Player";

    private InputAction _moveAction;
    private InputAction _adjustAngle;
    private InputAction _adjustForce;
    private InputAction _pickUp;
    private InputAction _talk;
    private InputAction _toggleQuestList;
    private InputAction _store;
    private InputAction _fishQuit;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();

        SetUpActions();
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void SetUpActions()
    {
        if (_playerInput.currentActionMap == null) return;
        _moveAction = _playerInput.actions["Move"]; 
        _adjustAngle = _playerInput.actions["AdjustAngle"];
        _adjustForce = _playerInput.actions["AdjustForce"];
        _pickUp = _playerInput.actions["PickUp"];
        _talk = _playerInput.actions["Talk"];
        _toggleQuestList = _playerInput.actions["ToggleQuestList"];
        _store = _playerInput.actions["Store"];
        _fishQuit = _playerInput.actions["Quit"];
    }


    private void UpdateInputs()
    {
        MoveInput = _moveAction.ReadValue<Vector2>();
        AdjustAngle = _adjustAngle.ReadValue<float>();
        AdjustForce = _adjustForce.ReadValue<float>();
        PickUp = _pickUp.WasPressedThisFrame();
        Talk = _talk.WasPressedThisFrame();
        ToggleQuestList = _toggleQuestList.WasPressedThisFrame();
        Store = _store.WasPressedThisFrame();
        fishQuit = _fishQuit.WasPressedThisFrame();
    }

    public void SwitchActionMap(string newMap)
    {
        if (_playerInput.currentActionMap.name == newMap) return;

        _playerInput.SwitchCurrentActionMap(newMap);
        currentActionMap = newMap;

        SetUpActions(); // Rebind actions for the new map
    }
    public string GetCurrentActionMap()
    {
        return currentActionMap;
    }
    public void EnableAction(string actionName)
    {
        var action = _playerInput.actions[actionName];
        if (action != null && !action.enabled)
            action.Enable();
    }

    public void DisableAction(string actionName)
    {
        var action = _playerInput.actions[actionName];
        if (action != null && action.enabled)
            action.Disable();
    }
}
