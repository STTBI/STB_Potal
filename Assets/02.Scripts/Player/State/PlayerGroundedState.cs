using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerController _player, StateMachine _stateMachine, CharacterController _charCtrl, string _animName, PlayerStateSystem _stateSystem) : base(_player, _stateMachine, _charCtrl, _animName, _stateSystem)
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

        if (InputManager.Instance.GetPlayerMovement().magnitude > 0)
            stateMachine.ChangeState(stateSystem.MoveState);
    }
}
