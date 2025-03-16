using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerStateSystem : MonoBehaviour
{
    private PlayerController player;

    #region Animator
    public Animator animBody;
    public Animator animShadow;
    public Animator animArms;
    #endregion

    public StateMachine stateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }

    public PlayerMoveState MoveState { get; private set; }
    public PlayerAirState AirState { get; private set; }


    private void Awake()
    {
        player = GetComponent<PlayerController>();
        stateMachine = new StateMachine();

        #region State
        IdleState = new PlayerIdleState(player, stateMachine, "Idle");
        MoveState = new PlayerMoveState(player, stateMachine, "Move");
        AirState = new PlayerAirState(player, stateMachine, "IsJump");
        #endregion
    }

    private void Start()
    {
        stateMachine.Initionalize(IdleState);
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.FixedUpdate();
    }

    private void Update()
    {
        stateMachine.CurrentState.Update();
    }

    public void SetTrigger(string animName)
    {
        animBody.SetTrigger(animName);
        animShadow.SetTrigger(animName);
        //animArms.SetTrigger(animName);
    }

    public void SetFloat(string valueName, float value)
    {
        animBody.SetFloat(valueName, value);
        animShadow.SetFloat(valueName, value);
        //animArms.SetFloat(valueName, value);
    }

    public void SetBool(string boolName, bool value)
    {
        animBody.SetBool(boolName, value);
        animShadow.SetBool(boolName, value);
        //animArms.SetBool(boolName, value);
    }
}
