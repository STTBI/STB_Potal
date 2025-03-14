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

    protected float timer;

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
        player.animBody.SetBool(animName, true);
        player.animShadow.SetBool(animName, true);
    }

    public virtual void Update()
    {
        timer -= Time.deltaTime;
        player.MoveMent.ApplyGravity();
        player.MoveMent.ApplyMovement();
    }

    public virtual void Exit()
    {
        player.animBody.SetBool(animName, false);
        player.animShadow.SetBool(animName, false);
    }
}
