using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Rigidbody rigid;
    [HideInInspector] public Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private InputManager inputManager;
    private Transform cameraTransform;

    private void OnValidate()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true;

        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // 그라운드에 접지 상태이면 y속도값을 0으로 초기화
        DtectedGround();
        ApplyMovement();
        ApplyGravity();
    }

    private void DtectedGround()
    {
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
    }

    private void ApplyGravity()
    {
        // Makes the player jump
        if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        groundedPlayer = (controller.Move(playerVelocity * Time.deltaTime) & CollisionFlags.Below) != 0;
    }

    private void ApplyMovement()
    {
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);
    }
}
