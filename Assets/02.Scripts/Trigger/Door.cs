using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator DoorAnim;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen"); // �̸� �ؽ� �� ����

    public int buttonCount;
    public bool[] isButtonPressed;

    private void OnValidate()
    { 
        buttonCount = 1;                         //�� ���� ��ư�� ������ ���� �������� ����
        
        DoorAnim = GetComponent<Animator>();    
    }

    private void Start()
    {
        DoorAnim.SetBool(IsOpen, false);
        isButtonPressed = new bool[buttonCount]; //��ư�� ������ŭ bool ������ ������ش�.
    }

    public void Decision()
    {
        //���� true��� Open
        //�ϳ��� false�� Close
        for (int i = 0; i < buttonCount; i++)
        {
            if (isButtonPressed[i] == false)
            {
                Close();
                return;
            }       
        }

        Open();
    }

    private void Open()
    {
        DoorAnim.SetBool(IsOpen, true);
    }

    private void Close()
    {
        DoorAnim.SetBool(IsOpen, false);
    }

}
