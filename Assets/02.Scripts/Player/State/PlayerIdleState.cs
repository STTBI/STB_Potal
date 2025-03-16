using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerController _player, StateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        movement.StopMove(rigid);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        movement.StopMove(rigid);
    }

    public override void Update()
    {
        base.Update();
        // 입력값이 있으면 움직임 상태 처리
        if (movement.Direction.magnitude != 0)
        {
            stateMachine.ChangeState(stateSystem.MoveState);
        }
    }
}
