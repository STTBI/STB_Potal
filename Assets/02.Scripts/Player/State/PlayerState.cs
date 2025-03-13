using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerController player;
    protected string animName;
    protected StateMachine stateMachine;
    protected PlayerStateSystem stateSystem;

    protected CharacterController charCtrl;

    public PlayerState(PlayerController _player, StateMachine _stateMachine, CharacterController _charCtrl, string _animName, PlayerStateSystem _stateSystem)
    {
        player = _player;
        stateMachine = _stateMachine;
        charCtrl = _charCtrl;
        animName = _animName;
        stateSystem = _stateSystem;
    }

    public virtual void Enter()
    {
        player.Anim.SetBool("Grounded", true);
    }

    public virtual void Update()
    {
        player.MoveMent.ApplyGravity();
    }

    public virtual void Exit()
    {
        player.Anim.SetBool("Grounded", false);
    }
}
