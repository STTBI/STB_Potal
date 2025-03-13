using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerController _player, StateMachine _stateMachine, CharacterController _charCtrl, string _animName, PlayerStateSystem _stateSystem) : base(_player, _stateMachine, _charCtrl, _animName, _stateSystem)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Anim.SetFloat("VelocityX", 0f);
        player.Anim.SetFloat("VelocityY", 0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
