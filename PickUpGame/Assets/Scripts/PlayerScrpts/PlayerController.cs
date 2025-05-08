using UnityEngine;
using UnityEngine.InputSystem; // <-- Important for InputAction!

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ItemHandler itemHandler;

    [SerializeField] private float moveSpeed = 4f;

    private Vector2 moveInput;
    private Vector3 forward, right;

    private InputSystem_Actions controls; 

    private void Awake()
    {
        controls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void Update()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();

        right = Quaternion.Euler(0, 90, 0) * forward;

        Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (inputDirection.magnitude > 0.1f)
        {
            Vector3 moveDirection = (right * inputDirection.x + forward * inputDirection.z).normalized;

            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * 10f);
            float speedMultiplier = itemHandler != null ? itemHandler.GetHeldItemSpeedMultiplier(): 1f;
            transform.position += moveDirection * moveSpeed * speedMultiplier * Time.deltaTime;

        }
    }
}
