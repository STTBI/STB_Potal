using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // 현재 스피드 변경
    private void ChangeSpeed()
    {
        if (Direction.y > 0f)
            CurrentSpeed = frontWalkSpeed;
        else if (Direction.y < 0f)
            CurrentSpeed = backWalkSpeed;
        else if (Direction.x != 0f)
            CurrentSpeed = sideWalkSpeed;
    }


    private void Update()
    {
        Direction = inputAction.Player.Movement.ReadValue<Vector2>();
    }
}
