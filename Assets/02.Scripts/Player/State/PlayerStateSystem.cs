using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerStateSystem : MonoBehaviour
{
    private PlayerController player;
    private CharacterController charCtrl;

    public StateMachine stateMachine { get; set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }


    private void Awake()
    {
        player = GetComponent<PlayerController>();
        charCtrl = GetComponent<CharacterController>();
        stateMachine = new StateMachine();

        #region State

        IdleState = new PlayerIdleState(player, stateMachine, charCtrl, "Idle", this);
        MoveState = new PlayerMoveState(player, stateMachine, charCtrl, "Move", this);

        #endregion
    }

    private void Start()
    {
        stateMachine.Initionalize(IdleState);
    }

    private void Update()
    {
        stateMachine.CurrentState.Update();
    }
}
