using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Animator buttonAnim;

    private void OnValidate()
    {
        buttonAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        switch(collider.gameObject.tag)
        {
            case "Player":
                Debug.Log("Player�� �浹");
                buttonAnim.SetBool("IsPressed", true);
                break;
            case "Obstacle":
                Debug.Log("Obstacle �浹");
                buttonAnim.SetBool("IsPressed", true);
                break;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Player":
                Debug.Log("Player ������");
                buttonAnim.SetBool("IsPressed", false);
                break;
            case "Obstacle":
                Debug.Log("Obstacle ������");
                buttonAnim.SetBool("IsPressed", false);
                break;
        }
    }




}

