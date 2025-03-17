using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCameraLook : CinemachineExtension
{
    [SerializeField] private Transform player;
    [SerializeField] private float sensivityX;
    [SerializeField] private float sensivityY;

    [SerializeField] private float clampAngle;

    public bool isInPortal = false;

    public bool isWalkInPortal = false;

    public Quaternion TargetRotation { private set; get; }

    private Vector3 startingRotation;

    public Vector2 Direction { get; set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                startingRotation.x += Direction.x * sensivityY * Time.deltaTime;
                startingRotation.y += Direction.y * sensivityX * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y , startingRotation.x, 0f);
                player.rotation = Quaternion.Euler(0f, startingRotation.x, 0f);
            }
        }
    }
    public void ResetTargetRotation()
{
    TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
}

}
