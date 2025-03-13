using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Door : MonoBehaviour
{
    [SerializeField] private Animator DoorAnim;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen"); // 미리 해시 값 저장

    public int triggerCount;

    private void OnValidate()
    {                         
        DoorAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        DoorAnim.SetBool(IsOpen, false);
    }

    public void Open()
    {
        DoorAnim.SetBool(IsOpen, true);
    }

    public void Close()
    {
        DoorAnim.SetBool(IsOpen, false);
    }
}
