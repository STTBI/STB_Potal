using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MovementHandler
{
    [Header("Move")]
    #region MoveData
    [SerializeField] private float backWalkSpeed;
    [SerializeField] private float sideWalkSpeed;
    [SerializeField] private float frontWalkSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpSlopeHeight;
    [SerializeField] public Vector3 currentVelocity;

    [Header("Velocity Limits")]
    [SerializeField] private float ySpeedMax = 25f;


    private Rigidbody rb;

    public bool isInPortal = false;

    #endregion

    private Vector3 moveDirection;
    private Vector3 gravity;

    // 프로퍼티
    public bool IsJump { get; private set; }

    private void OnValidate()
    {
        CurrentSpeed = frontWalkSpeed;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;

        // 회전값을 int로 변환
        int rotX = Mathf.RoundToInt(currentRotation.x);
        int rotZ = Mathf.RoundToInt(currentRotation.z);

        // X, Z 회전값이 0에 가까운지 확인 (0과의 비교)
        if (rotX != 0 || rotZ != 0)
        {
            // X, Z 회전값을 0으로 부드럽게 변경
            float targetX = Mathf.LerpAngle(currentRotation.x, 0f, Time.deltaTime * 5f);
            float targetZ = Mathf.LerpAngle(currentRotation.z, 0f, Time.deltaTime * 5f);

            // 이동 방향의 반대 방향을 나타내는 Y값 계산
            float targetY = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg + 180f;

            transform.rotation = Quaternion.Euler(targetX, targetY, targetZ);
        }

        // 현재 속도 가져오기
        Vector3 velocity = rb.velocity;

        // Y축 속도 제한 (ySpeedMax 이하로)
        velocity.y = Mathf.Sign(velocity.y) * Mathf.Min(Mathf.Abs(velocity.y), ySpeedMax);  // 절댓값으로 클램프

        // 제한된 속도를 Rigidbody에 다시 반영
        rb.velocity = velocity;
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.F))
        {
            Time.timeScale = 0.3f;
        }else{
            Time.timeScale = 1;
        }
    }



    // 현재 스피드 변경
    public void ChangeSpeed()
    {
        if (Direction.y > 0f)
            CurrentSpeed = frontWalkSpeed;
        else if (Direction.y < 0f)
            CurrentSpeed = backWalkSpeed;
        else if (Direction.x != 0f)
            CurrentSpeed = sideWalkSpeed;
    }

    public bool OnMove()
    {
        if(isInPortal)
            return false;

        bool isGround = CheckGround();
        bool onSlope = IsOnSlope(); // 경사면 체크
        moveDirection = Vector3.right * Direction.x + Vector3.forward * Direction.y;
        moveDirection = transform.TransformDirection(moveDirection); // 로컬 좌표에서 월드 좌표로 변경

        if (isGround && onSlope)
        {
            moveDirection = AdjustDirectionToSlope(moveDirection);
            gravity = Vector3.zero;
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }

        if(rb.useGravity)
            gravity += Vector3.down * 9.81f * Time.fixedDeltaTime;
        
        rb.velocity = moveDirection * CurrentSpeed + gravity;
        currentVelocity = rb.velocity;
        return Direction.magnitude > 0f;
    }

    public void ZeroGravity()
    {
        if(!isInPortal)
            {
                 gravity = Vector3.zero;
            }
    }

    public void StopMove()
    {
        if(!isInPortal)
        {
            rb.velocity = Vector3.up * rb.velocity.y;
        }
    }

    public bool OnJump()
    {
        
        if(IsJump)
        {
            rb.useGravity = true;
            gravity = new Vector3(gravity.x, jumpHeight, gravity.z);
            rb.AddForce(gravity, ForceMode.Impulse);
            IsJump = false;
            return true;
        }

        return false;
    }

    public void CanJump()
    {
        if(CheckGround())
        {
            isInPortal = false;
            IsJump = true;
        }
    }
}
