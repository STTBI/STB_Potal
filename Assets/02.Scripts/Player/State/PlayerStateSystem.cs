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
    #endregion

    public StateMachine stateMachine { get; set; }
    public PlayerIdleState IdleState { get; private set; }

    public PlayerMoveState MoveState { get; private set; }


    private void Awake()
    {
        player = GetComponent<PlayerController>();
        stateMachine = new StateMachine();

        #region State
        IdleState = new PlayerIdleState(player, stateMachine, "Idle", this);
        MoveState = new PlayerMoveState(player, stateMachine, "Move", this);
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
}
