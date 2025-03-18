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
        //ICheckTrigger瑜??곸냽 諛쏆? 紐⑤뱺 ?먯떇?ㅼ쓣 李얠븘?⑤떎.
        triggerObject = GetComponentsInChildren<ITriggerObject>();

        //?몃━嫄곗? ?곌껐?댁쨪 臾몄쓣 李얠븘?⑤떎. (臾몄? ??媛쒖뿬????
        door = GetComponentInChildren<Door>();

        //?뺤씤??
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
