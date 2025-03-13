using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TriggerManager : MonoBehaviour
{
    public Button button;


    public event Action PressAction;
    public event Action ReleaseAction;

    public void Start()
    {
        
    }


}
