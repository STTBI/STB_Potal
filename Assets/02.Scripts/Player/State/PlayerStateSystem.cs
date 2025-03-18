using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerStateSystem : MonoBehaviour
{
    private PlayerController player;

    // ?곹깭 遺덈뒫 肄붾（??
    private Coroutine dontCoroutine;

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
        if(!player.IsDeath)
            stateMachine.CurrentState.FixedUpdate();
    }

    private void Update()
    {
        if (!player.IsDeath)
            stateMachine.CurrentState.Update();
        else if(dontCoroutine == null)
        {
            SetTrigger("Death");
            player.Rigid.velocity = Vector3.zero;
            dontCoroutine = StartCoroutine(CanRestart());
        }
    }

    private IEnumerator CanRestart()
    {
        float curDelay = 3f;
        SetBool("IsDeath", true);
        while (curDelay > 0f)
        {
            curDelay -= Time.deltaTime;
            yield return null;
        }

        player.transform.position = player.SavePoint;
        player.IsDeath = false;
        SetBool("IsDeath", false);
        player.CameraLook.fpsViewCamera.gameObject.SetActive(true);
        player.CameraLook.deathCamera.gameObject.SetActive(false);
        dontCoroutine = null;
    }

    public void SetTrigger(string animName)
    {
        animBody.SetTrigger(animName);
        animShadow.SetTrigger(animName);
        animArms.SetTrigger(animName);
    }

    public void SetFloat(string valueName, float value)
    {
        animBody.SetFloat(valueName, value);
        animShadow.SetFloat(valueName, value);
        animArms.SetFloat(valueName, value);
    }

    public void SetBool(string boolName, bool value)
    {
        animBody.SetBool(boolName, value);
        animShadow.SetBool(boolName, value);
        animArms.SetBool(boolName, value);
    }
}
