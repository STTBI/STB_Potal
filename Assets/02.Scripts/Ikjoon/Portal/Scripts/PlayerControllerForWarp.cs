using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerForWarp : PortalableObject
{
    private PlayerCameraControl cameraMove;

    protected override void Awake()
    {
        base.Awake();

        cameraMove = GetComponent<PlayerCameraControl>();
    }

    public override void Warp()
    {
        base.Warp();
        cameraMove.ResetTargetRotation();
    }
}
