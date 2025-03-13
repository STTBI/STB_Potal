using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator DoorAnim;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen"); // 미리 해시 값 저장

    public int buttonCount;
    public bool[] isButtonPressed;

    private void OnValidate()
    { 
        buttonCount = 1;                         //몇 개의 버튼을 눌러야 문이 열리는지 설정
        
        DoorAnim = GetComponent<Animator>();    
    }

    private void Start()
    {
        DoorAnim.SetBool(IsOpen, false);
        isButtonPressed = new bool[buttonCount]; //버튼의 갯수만큼 bool 변수를 만들어준다.
    }

    public void Decision()
    {
        //전부 true라면 Open
        //하나라도 false면 Close
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
