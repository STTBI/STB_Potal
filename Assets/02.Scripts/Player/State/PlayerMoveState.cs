using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(PlayerController _player, StateMachine _stateMachine, CharacterController _charCtrl, string _animName, PlayerStateSystem _stateSystem) : base(_player, _stateMachine, _charCtrl, _animName, _stateSystem)
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

    public override void Update()
    {
        base.Update();

        player.MoveMent.ApplyMovement();
        player.Anim.SetFloat("VelocityX", charCtrl.velocity.x);
        player.Anim.SetFloat("VelocityY", charCtrl.velocity.z);
        Debug.Log(charCtrl.velocity.x);

        if (InputManager.Instance.GetPlayerMovement().magnitude == 0f)
            stateMachine.ChangeState(stateSystem.IdleState);
    }
}
