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

    // ������Ƽ
    public bool IsJump { get; private set; }

    private void OnValidate()
    {
        CurrentSpeed = frontWalkSpeed;
    }

    // ���� ���ǵ� ����
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
        bool onSlope = IsOnSlope(); // ���� üũ
        moveDirection = Vector3.right * Direction.x + Vector3.forward * Direction.y;
        moveDirection = transform.TransformDirection(moveDirection); // ���� ��ǥ���� ���� ��ǥ�� ����
        moveDirection = (onSlope) ? AdjustDirectionToSlope(moveDirection) : moveDirection; // �������͹��� : �������

        // ���� ���� ���ϰ��� y�� ���ԵǾ �����⿡ �߷��� 0���� ������ش�.
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
