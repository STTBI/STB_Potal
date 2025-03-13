using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator DoorAnim;

    private void OnValidate()
    {
        DoorAnim = GetComponent<Animator>();
    }

}
