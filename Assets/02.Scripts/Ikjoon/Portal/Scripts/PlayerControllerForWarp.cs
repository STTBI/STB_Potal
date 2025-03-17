using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerForWarp : PortalableObject
{
    private PlayerCameraLook cameraLook;

    protected override void Awake()
    {
        base.Awake();

        cameraLook = GetComponent<PlayerCameraLook>();
    }

    public override void Warp()
    {
        base.Warp();
        cameraLook.ResetTargetRotation();
    }
}
