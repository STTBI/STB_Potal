using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionButton : MonoBehaviour, ITriggerObject
{
    [SerializeField] private Animator buttonAnim;
    private static readonly int IsPressed = Animator.StringToHash("IsPressed"); // 미리 해시 값 저장

    private bool check;
    public bool TriggerCheck
    {
        get
        {
            return check;
        }
        set
        {
            check = value;

            if (check)
            {
                Enter();
                Invoke("Exit", 3.0f);
            }
        }
    }
    private void OnValidate()
    {
        buttonAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        buttonAnim.SetBool(IsPressed, false);
    }
    
    public void Enter()
    {
        buttonAnim.SetBool(IsPressed, TriggerCheck);
    }

    public void Exit()
    {
        buttonAnim.SetBool(IsPressed, TriggerCheck);
        TriggerCheck = false;
    }

}
