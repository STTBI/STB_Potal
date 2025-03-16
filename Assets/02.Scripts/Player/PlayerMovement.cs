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
    #endregion

    private Vector3 moveDirection;
    private Vector3 moveTransform;

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
        if(Direction.magnitude > 0f)
        {
            bool onSlope = IsOnSlope(); // ���� üũ
            moveDirection = Vector3.right * Direction.x + Vector3.forward * Direction.y;
            moveDirection = transform.TransformDirection(moveDirection); // ���� ��ǥ���� ���� ��ǥ�� ����
            moveDirection = (onSlope) ? AdjustDirectionToSlope(moveDirection) : moveDirection; // �������͹��� : �������

            // ���� ���� ���ϰ��� y�� ���ԵǾ �����⿡ �߷��� 0���� ������ش�.
            Vector3 gravity = (onSlope) ? Vector3.zero : Vector3.down * Mathf.Abs(rigid.velocity.y);

            Vector3 endPoint = transform.position + moveDirection * CurrentSpeed * Time.fixedDeltaTime;
            Debug.Log(endPoint);
            transform.position = Vector3.Lerp(transform.position, endPoint, Time.fixedDeltaTime * 19f);
            /*if (CheckGround())
            {
                Vector3 endPoint = transform.position + moveDirection * CurrentSpeed * Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, endPoint, Time.fixedDeltaTime * 19f);
                //rigid.velocity = moveDirection * CurrentSpeed + gravity;            
            }
            else
            {
                Vector3 endPoint = transform.position + moveDirection * CurrentSpeed * Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, endPoint, Time.fixedDeltaTime * 19f);
                //rigid.velocity = new Vector3(moveDirection.x * CurrentSpeed, rigid.velocity.y, moveDirection.z * CurrentSpeed);
            }*/

        }
    }

    public void StopMove(Rigidbody rigid)
    {
        Vector3 newVector = Vector3.up * rigid.velocity.y;
        rigid.velocity = newVector;
    }
    public void OnJump(Rigidbody rigid)
    {
        if(CheckGround() && IsJump)
        {
            rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            IsJump = false;
        }
    }

    public void CanJump()
    {
        if(CheckGround() && !IsJump)
            IsJump = true;
    }
}
