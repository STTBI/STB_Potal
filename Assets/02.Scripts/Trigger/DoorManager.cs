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
    public string GetinteractText();    //È­¸é¿¡ ¶ç¿öÁÙ ÅØ½ºÆ®¸¦ ¹İÈ¯ÇÏ´Â ÇÔ¼ö
    public void OnInteract();           //»óÈ£ÀÛ¿ëÀ» ÇßÀ» ¶§ ½ÇÇàÇÒ ÇÔ¼ö
}

public class DoorManager : MonoBehaviour
{
    public ITriggerObject[] triggerObject;
    public Door door;

    [Header("Debug")]
    [SerializeField] private int TriggerObjectCount = 0;

    private void OnValidate()
    {
        //ICheckTriggerç‘œ??ê³¸ëƒ½ è«›ì†? ï§â‘¤ë±º ?ë¨¯ë–‡?ã…¼ì“£ ï§¡ì– ë¸˜?â‘¤ë–.
        triggerObject = GetComponentsInChildren<ITriggerObject>();

        //?ëªƒâ”å«„ê³—? ?ê³Œê»?ëŒì¨ª è‡¾ëª„ì“£ ï§¡ì– ë¸˜?â‘¤ë–. (è‡¾ëª„? ??åª›ì’–ë¿¬????
        door = GetComponentInChildren<Door>();

        //?ëº¤ì”¤??
        TriggerObjectCount = triggerObject.Length;
    }

    private void Update()
    {
        Decision();
    }

    public void Decision()
    {
        //?ê¾¨? true?ì‡°ãˆƒ Open
        //?ì„êµ¹?ì‡°ë£„ falseï§?Close
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
