using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private InputManager inputManager;

    private CharacterController controller;
    private Rigidbody rigid;
    private bool groundedPlayer;

    [Header("Move Info")]
    [SerializeField] private float backWalkSpeed = 1.0f;
    [SerializeField] private float sideWalkSpeed = 2.0f;
    [SerializeField] private float frontWalkSpeed = 3.0f;
    [SerializeField] private float RunSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    [Header("Slope")]
    private const float RAY_DISTANCE = 2f;
    private float maxSlope;
    private RaycastHit slopeHit;
    public LayerMask groundLayer;


    [HideInInspector] public Vector3 playerVelocity;
    private Vector3 moveDirection;
    
    public float CurrentSpeed { get; private set; }
    public Vector3 AddForceMove { get; set; } = Vector3.zero;
    public Vector3 Direction { get; private set; }

    private void OnValidate()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.isKinematic = true;

        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        Direction = inputManager.GetPlayerMovement();
        ChangeSpeed();
        maxSlope = controller.slopeLimit;
    }

    private void ChangeSpeed()
    {
        if (Direction.y > 0f)
            CurrentSpeed = frontWalkSpeed;
        else if (Direction.y < 0f)
            CurrentSpeed = backWalkSpeed;
        else if (Direction.x != 0f)
            CurrentSpeed = sideWalkSpeed;
    }

    // 경사면 체크
    public bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(ray, out slopeHit, RAY_DISTANCE, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlope;
        }

        return false;
    }

    // 방향 벡터 추출
    private Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public void DtectedGround()
    {
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }

    public bool ApplyJump()
    {
        if (inputManager.PlayerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            return true;
        }

        return false;
    }

    public void ApplyGravity()
    {
        // Makes the player jump
        DtectedGround();
        playerVelocity.y += gravityValue * Time.deltaTime;
        groundedPlayer = (controller.Move(Vector3.up * playerVelocity.y * Time.deltaTime) & CollisionFlags.Below) != 0;
    }

    public void ApplyMovement()
    {
        bool isOnSlope = IsOnSlope();
        moveDirection = new Vector3(Direction.x, 0, Direction.y);
        moveDirection = transform.TransformDirection(moveDirection) * CurrentSpeed;
        Vector3 velocity = isOnSlope ? AdjustDirectionToSlope(moveDirection) * CurrentSpeed : moveDirection;
        velocity = velocity * Time.deltaTime;
        
        AddForceMove = Vector3.Lerp(AddForceMove, moveDirection, Time.deltaTime * 19f); // testScalar
        playerVelocity = new Vector3(velocity.x, playerVelocity.y, velocity.z);
        groundedPlayer = (controller.Move(AddForceMove * Time.deltaTime) & CollisionFlags.Below) != 0;

        Debug.Log(AddForceMove);
    }
}
