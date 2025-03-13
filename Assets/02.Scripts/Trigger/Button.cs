using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Animator buttonAnim;
    private static readonly int IsPressed = Animator.StringToHash("IsPressed"); // ¹Ì¸® ÇØ½Ã °ª ÀúÀå

    private void OnValidate()
    {
        buttonAnim = GetComponent<Animator>();
        buttonAnim.SetBool(IsPressed, true);
    }

    private void OnTriggerEnter(Collider collider)
    {
        switch(collider.gameObject.tag)
        {
            case "Player":
                Debug.Log("Player¿Í Ãæµ¹");
                buttonAnim.SetBool(IsPressed, true);
                break;
            case "Obstacle":
                Debug.Log("Obstacle Ãæµ¹");
                buttonAnim.SetBool(IsPressed, true);
                break;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Player":
                Debug.Log("Player ¶³¾îÁü");
                buttonAnim.SetBool(IsPressed, false);
                break;
            case "Obstacle":
                Debug.Log("Obstacle ¶³¾îÁü");
                buttonAnim.SetBool(IsPressed, false);
                break;
        }
    }

    public void Press()
    {
        buttonAnim.SetBool(IsPressed, false);
    }

    public void Release()
    {
        buttonAnim.SetBool(IsPressed, true);
    }




}

