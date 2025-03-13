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
        //ICheckTrigger를 상속 받은 모든 자식들을 찾아온다.
        triggerObject = GetComponentsInChildren<ITriggerObject>();

        //트리거와 연결해줄 문을 찾아온다. (문은 한 개여야 함)
        door = GetComponentInChildren<Door>();

        //확인용
        TriggerObjectCount = triggerObject.Length;
    }

    private void Update()
    {
        Decision();
    }

    public void Decision()
    {
        //전부 true라면 Open
        //하나라도 false면 Close
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
