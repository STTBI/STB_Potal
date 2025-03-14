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

        player.animBody.SetFloat("VelocityX", player.MoveMent.Direction.x * player.MoveMent.AddForceMove.magnitude);
        player.animBody.SetFloat("VelocityY", player.MoveMent.Direction.y * player.MoveMent.AddForceMove.magnitude);

        player.animShadow.SetFloat("VelocityX", player.MoveMent.Direction.x * player.MoveMent.AddForceMove.magnitude);
        player.animShadow.SetFloat("VelocityY", player.MoveMent.Direction.y * player.MoveMent.AddForceMove.magnitude);

        if (player.MoveMent.AddForceMove.magnitude == 0f)
            stateMachine.ChangeState(stateSystem.IdleState);
    }
}
