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
    public PlayerJumpStartState JumpStartState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerLandState landState { get; private set; }


    private void Awake()
    {
        player = GetComponent<PlayerController>();
        charCtrl = GetComponent<CharacterController>();
        stateMachine = new StateMachine();

        #region State

        IdleState = new PlayerIdleState(player, stateMachine, charCtrl, "Idle", this);
        MoveState = new PlayerMoveState(player, stateMachine, charCtrl, "Move", this);
        JumpStartState = new PlayerJumpStartState(player, stateMachine, charCtrl, "IsJump", this);
        AirState = new PlayerAirState(player, stateMachine, charCtrl, "Air", this);
        landState = new PlayerLandState(player, stateMachine, charCtrl, "Land", this);

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
