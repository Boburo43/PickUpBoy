using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ItemHandler itemHandler;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float pushForce = 0.2f;
    [SerializeField] private float gravityValue = -9.81f;

    private CharacterController characterController;
    private Vector3 forward, right;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void LateUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        groundedPlayer = characterController.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // Keeps player grounded firmly
        }

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();

        right = Quaternion.Euler(0, 90, 0) * forward;

        Vector2 moveInput = UserInputManager.instance.MoveInput;
        Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 finalMove = Vector3.zero;

        if (inputDirection.magnitude > 0.1f)
        {
            Vector3 moveDirection = (right * inputDirection.x + forward * inputDirection.z).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            float speedMultiplier = itemHandler != null ? itemHandler.GetHeldItemSpeedMultiplier() : 1f;
            finalMove = moveDirection * moveSpeed * speedMultiplier;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        characterController.Move((finalMove + playerVelocity) * Time.deltaTime);
    }

     /* private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body != null && !body.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }*/
}
