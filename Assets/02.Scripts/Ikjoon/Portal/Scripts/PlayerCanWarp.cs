using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerCanWarp : PortalableObject
{
    private PlayerCtrl playerCtrl;  
    protected override void Awake()
    {
        base.Awake();
        
        // PlayerCtrl 컴포넌트 할당
        playerCtrl = GetComponent<PlayerCtrl>();
    }

    public override void Warp()
    {
        base.Warp();
        
        // PlayerCtrl에서 카메라 회전 리셋 처리
        //playerCtrl.ResetCameraRotation();
    }
}