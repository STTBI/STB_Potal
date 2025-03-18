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
    public string GetinteractText();    //화면에 띄워줄 텍스트를 반환하는 함수
    public void OnInteract();           //상호작용을 했을 때 실행할 함수
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

    public void Decision()
    {
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
