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


    private Rigidbody rb;

    public bool isInPortal;

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
        // Quaternion targetRotation = quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        // rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5f));
        // 현재 회전값
    Vector3 currentRotation = transform.rotation.eulerAngles;

    // 회전값을 (0, y, 0)으로 되돌림
    transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
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

    public bool OnMove(Rigidbody rigid)
    {
        bool isGround = CheckGround();
        bool onSlope = IsOnSlope(); // 경사면 체크
        moveDirection = Vector3.right * Direction.x + Vector3.forward * Direction.y;
        moveDirection = transform.TransformDirection(moveDirection); // 로컬 좌표에서 월드 좌표로 변경

        if (isGround && onSlope)
        {
            moveDirection = AdjustDirectionToSlope(moveDirection);
            gravity = Vector3.zero;
            rigid.useGravity = false;
        }
        else
        {
            rigid.useGravity = true;
        }

        if(rigid.useGravity)
            gravity += Vector3.down * 9.81f * Time.fixedDeltaTime;
        
        rigid.velocity = moveDirection * CurrentSpeed + gravity;
        currentVelocity = rigid.velocity;
        return Direction.magnitude > 0f;
    }

    public void ZeroGravity()
    {
        gravity = Vector3.zero;
    }

    public void StopMove(Rigidbody rigid)
    {
        rigid.velocity = Vector3.up * rigid.velocity.y;
    }

    public bool OnJump(Rigidbody rigid)
    {
        if(IsJump)
        {
            rigid.useGravity = true;
            gravity = new Vector3(gravity.x, jumpHeight, gravity.z);
            rigid.AddForce(gravity, ForceMode.Impulse);
            IsJump = false;
            return true;
        }

        return false;
    }

    public void CanJump()
    {
        if(CheckGround() && !IsJump)
            IsJump = true;
    }
}
