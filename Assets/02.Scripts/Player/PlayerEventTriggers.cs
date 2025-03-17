using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventTriggers : MonoBehaviour
{
    public void StepEvent(int index)
    {
        AudioManager.Instance.PlaySFX($"Step{index:D2}", false);
    }
}
