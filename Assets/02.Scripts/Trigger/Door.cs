using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Door : MonoBehaviour
{
    [SerializeField] private Animator DoorAnim;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen"); // �̸� �ؽ� �� ����

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
