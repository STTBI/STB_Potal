using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventTriggers : MonoBehaviour
{
    PlayerMovement movement;

    private void OnValidate()
    {
        movement = GetComponentInParent<PlayerMovement>();
    }

    public void StepEvent(int index)
    {
        if(Mathf.Abs(movement.Direction.x) > 0.9f || Mathf.Abs(movement.Direction.y) > 0.9f)
        {
            AudioManager.Instance.PlaySFX($"Step{index}");
        }
    }

    public void FireEvent()
    {
        AudioManager.Instance.PlaySFX("Fire");
    }

    public void LandStepEvent(int index)
    {
        AudioManager.Instance.PlaySFX($"Step{index}");
    }
}
