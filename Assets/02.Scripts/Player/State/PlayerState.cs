using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    // 유니티 컴포넌트
    protected Rigidbody rigid;

    // 스크립트 컴포넌트
    protected PlayerController player;
    protected PlayerMovement movement;
    protected PlayerStateSystem stateSystem;

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

        movement = player.Movement;
        stateSystem = player.StateSystem;

        rigid = player.Rigid;
    }

    public virtual void Enter()
    {

    }

    public virtual void FixedUpdate()
    {
        movement.OnMove(rigid);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateSystem.SetTrigger("Fire");
        }

        if (!movement.CheckGround())
            rigid.useGravity = true;

        if(rigid.velocity.y < 0f)
        {
            stateMachine.ChangeState(stateSystem.AirState);
            return;
        }
    }

    public virtual void Exit()
    {

    }
}
