using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerController _player, StateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if (movement.CheckGround())
        {
            stateSystem.SetFloat("VerticalY", 0f);
            stateMachine.ChangeState(stateSystem.IdleState);
            return;
        }
        else
        {
            stateSystem.SetFloat("VerticalY", rigid.velocity.y);
        }
    }
}
