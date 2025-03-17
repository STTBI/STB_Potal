using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerController _player, StateMachine _stateMachine, string _animName) : base(_player, _stateMachine, _animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateSystem.SetBool("Grounded", true);
    }

    public override void Exit()
    {
        base.Exit();
        stateSystem.SetBool("Grounded", false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        movement.ZeroGravity();

        stateSystem.SetBool("Move", movement.OnMove(rigid));

        if (movement.OnJump(rigid))
        {
            stateSystem.SetTrigger("Jump");
            stateMachine.ChangeState(stateSystem.AirState);
            return;
        }    
    }

    public override void Update()
    {
        base.Update();
        movement.ChangeSpeed();
        stateSystem.SetFloat("DirX", movement.Direction.x);
        stateSystem.SetFloat("DirY", movement.Direction.y);
    }
}
