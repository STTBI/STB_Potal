using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    #endregion

    private Vector3 moveDirection;

    // 프로퍼티
    public bool IsJump { get; private set; }

    private void OnValidate()
    {
        CurrentSpeed = frontWalkSpeed;
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

    public void OnMove(Rigidbody rigid)
    {
        bool onSlope = IsOnSlope(); // 경사면 체크
        moveDirection = Vector3.right * Direction.x + Vector3.forward * Direction.y;
        moveDirection = transform.TransformDirection(moveDirection); // 로컬 좌표에서 월드 좌표로 변경
        moveDirection = (onSlope) ? AdjustDirectionToSlope(moveDirection) : moveDirection; // 법선벡터방향 : 월드방향

        // 법선 벡터 리턴값이 y축 포함되어서 나오기에 중력을 0으로 만들어준다.
        Vector3 gravity = (onSlope) ? Vector3.zero : Vector3.down * Mathf.Abs(rigid.velocity.y);


        if (CheckGround())
        {
            rigid.velocity = moveDirection * CurrentSpeed + gravity;
        }
        else
        {
            rigid.velocity = new Vector3(moveDirection.x * CurrentSpeed, rigid.velocity.y, moveDirection.z * CurrentSpeed);
        }
    }

    public void StopMove(Rigidbody rigid)
    {
        rigid.velocity = Vector3.up * rigid.velocity.y;
    }
    public void OnJump(Rigidbody rigid)
    {
        if(IsJump)
        {
            rigid.AddForce(Vector3.up * (IsOnSlope() ? jumpSlopeHeight : jumpHeight), ForceMode.Impulse);
            IsJump = false;
        }
    }

    public void CanJump()
    {
        if(CheckGround() && !IsJump)
            IsJump = true;
    }
}
