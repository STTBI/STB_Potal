using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput
{
    Player_AC playerAC;

    private Transform playerTrans;
    private PlayerMovement movement;
    private PlayerCameraControl cameraLook;
    private PortalGun portalGun;

    public PlayerInput(PlayerController player)
    {
        playerAC = new Player_AC();

        playerTrans = player.transform;
        movement = player.Movement;
        cameraLook = player.CameraLook;
        portalGun = player.CurrentGun;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Initionalize()
    {
        playerAC.Player.Movement.performed += ctx => movement.Direction = ctx.ReadValue<Vector2>();
        playerAC.Player.Movement.canceled += ctx => movement.Direction = Vector2.zero;
        playerAC.Player.Jump.started += ctx => movement.CanJump();

        playerAC.Player.Look.performed += ctx => cameraLook.Direction = ctx.ReadValue<Vector2>();
        playerAC.Player.LeftFire.started += ctx => portalGun.Fire(0, playerTrans.position, playerTrans.forward, 250.0f);
        playerAC.Player.RightFire.started += ctx => portalGun.Fire(1, playerTrans.position, playerTrans.forward, 250.0f);
    }

    public void Enable()
    {
        playerAC.Enable();
    }

    public void Disable()
    {
        playerAC.Disable();
    }
}
