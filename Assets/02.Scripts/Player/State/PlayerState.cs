using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    // 유니티 컴포넌트
    protected Rigidbody rigid;

    // 스크립트 컴포넌트
    protected PlayerController player;
    protected MovementHandler movement;
    protected PlayerStateSystem stateSystem;
    protected Player_AC inputAction;

    // 유한 상태 기계
    protected string animName;
    protected StateMachine stateMachine;

    // 행동 타이머
    protected float stateTimer;

    public PlayerState(PlayerController _player, StateMachine _stateMachine, string _animName)
    {
        player = _player;
        stateMachine = _stateMachine;
        animName = _animName;

        inputAction = player.PlayerAC;
        movement = player.Movement;
        stateSystem = player.StateSystem;

        rigid = player.Rigid;
    }

    public virtual void Enter()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {

    }
}
