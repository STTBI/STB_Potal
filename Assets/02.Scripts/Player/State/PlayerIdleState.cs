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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.MoveMent.AddForceMove.magnitude > 0)
            player.MoveMent.AddForceMove = Vector3.Lerp(player.MoveMent.AddForceMove, Vector3.zero, Time.deltaTime);

        if (InputManager.Instance.GetPlayerMovement().magnitude > 0)
            stateMachine.ChangeState(stateSystem.MoveState);
    }
}
