using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpStartState : PlayerState
{
    public PlayerJumpStartState(PlayerController _player, StateMachine _stateMachine, CharacterController _charCtrl, string _animName, PlayerStateSystem _stateSystem) : base(_player, _stateMachine, _charCtrl, _animName, _stateSystem)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0.5f;

        player.MoveMent.ApplyJump();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (timer < 0f && player.MoveMent.groundedPlayer)
            stateMachine.ChangeState(stateSystem.IdleState);
    }
}
