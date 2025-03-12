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
                Debug.Log("Player¿Í Ãæµ¹");
                buttonAnim.SetBool("IsPressed", true);
                break;
            case "Obstacle":
                Debug.Log("Obstacle Ãæµ¹");
                buttonAnim.SetBool("IsPressed", true);
                break;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Player":
                Debug.Log("Player ¶³¾îÁü");
                buttonAnim.SetBool("IsPressed", false);
                break;
            case "Obstacle":
                Debug.Log("Obstacle ¶³¾îÁü");
                buttonAnim.SetBool("IsPressed", false);
                break;
        }
    }




}

