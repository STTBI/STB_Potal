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

public class DoorManager : MonoBehaviour
{
    public ITriggerObject[] triggerObject;
    public Door door;

    [Header("Debug")]
    [SerializeField] private int TriggerObjectCount = 0;

    private void OnValidate()
    {
        //ICheckTrigger�� ��� ���� ��� �ڽĵ��� ã�ƿ´�.
        triggerObject = GetComponentsInChildren<ITriggerObject>();

        //Ʈ���ſ� �������� ���� ã�ƿ´�. (���� �� ������ ��)
        door = GetComponentInChildren<Door>();

        //Ȯ�ο�
        TriggerObjectCount = triggerObject.Length;
    }

    private void Update()
    {
        Decision();
    }

    public void Decision()
    {
        //���� true��� Open
        //�ϳ��� false�� Close
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
