using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public interface ITriggerObject
{
    bool TriggerCheck { get; set; }
    void Enter();
    void Exit();
}

public interface IInteractable
{
    public string GetinteractText();    //ȭ�鿡 ����� �ؽ�Ʈ�� ��ȯ�ϴ� �Լ�
    public void OnInteract();           //��ȣ�ۿ��� ���� �� ������ �Լ�
}

public class DoorManager : MonoBehaviour
{
    public ITriggerObject[] triggerObject;
    public Door door;

    [Header("Debug")]
    [SerializeField] private int TriggerObjectCount = 0;

    private void OnValidate()
    {
        triggerObject = GetComponentsInChildren<ITriggerObject>();

        door = GetComponentInChildren<Door>();

        TriggerObjectCount = triggerObject.Length;
    }

    private void Update()
    {
        Decision();
    }

    public void Decision()
    {
        //?꾨? true?쇰㈃ Open
        //?섎굹?쇰룄 false硫?Close
        //?袁? true??겹늺 Open
        //??롪돌??곕즲 false筌?Close
        for (int i = 0; i < triggerObject.Length; i++)
        {
            if (triggerObject[i].TriggerCheck == false)
            {
                door.Close();
                return;
            }
        }
        door.Open();
    }
}
