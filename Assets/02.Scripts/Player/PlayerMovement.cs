using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Rigidbody rigid;
    private bool groundedPlayer;

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private InputManager inputManager;
    private Transform cameraTransform;

    
    [HideInInspector] public Vector2 inputDirection;
    [HideInInspector] public Vector3 playerVelocity;
    [HideInInspector] public Vector3 moveDirection;

    private void OnValidate()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true;

        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        inputDirection = inputManager.GetPlayerMovement();
    }

    public void DtectedGround()
    {
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
    }

    public void ApplyGravity()
    {
        // Makes the player jump
        /*if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        groundedPlayer = (controller.Move(playerVelocity * Time.deltaTime) & CollisionFlags.Below) != 0;*/
        moveDirection.y += gravityValue * Time.deltaTime;
        groundedPlayer = (controller.Move(Vector3.up * moveDirection.y * Time.deltaTime) & CollisionFlags.Below) != 0;
    }

    public void ApplyMovement()
    {
        /*Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);*/
        moveDirection = new Vector3(inputDirection.x, -1f, inputDirection.y);
        if (groundedPlayer)
            moveDirection = transform.TransformDirection(moveDirection) * playerSpeed;

        groundedPlayer = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }
}
