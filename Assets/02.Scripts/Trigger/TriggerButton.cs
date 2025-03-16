using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Build;
using UnityEngine;

public class TriggerButton : MonoBehaviour, ITriggerObject
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

            if (check) Enter();
            else Exit();
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

    private void OnTriggerEnter(Collider collider)
    {
        switch(collider.gameObject.tag)
        {
            case "Player":
                Debug.Log("Player와 충돌함");
                TriggerCheck = true;
                break;

            case "Obstacle":
                Debug.Log("Obstacle와 충돌함");
                TriggerCheck = true;
                break;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Player":
                Debug.Log("Player 떨어짐");
                TriggerCheck = false;
                break;
            case "Obstacle":
                Debug.Log("Obstacle 떨어짐");
                TriggerCheck = false;
                break;
        }
    }

    public void Enter()
    {    
        buttonAnim.SetBool(IsPressed, TriggerCheck);
    }

    public void Exit()
    {       
        buttonAnim.SetBool(IsPressed, TriggerCheck);
    }
}

