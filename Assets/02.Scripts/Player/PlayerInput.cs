using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    Player_AC playerAC;

    
    PlayerMovement movement;
    PlayerCameraControl cameraLook;

    public PlayerInput(PlayerController player)
    {
        playerAC = new Player_AC();
        movement = player.Movement;
        cameraLook = player.CameraLook;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Initionalize()
    {
        playerAC.Player.Movement.performed += ctx => movement.Direction = ctx.ReadValue<Vector2>();
        playerAC.Player.Movement.canceled += ctx => movement.Direction = Vector2.zero;
        playerAC.Player.Jump.started += ctx => movement.CanJump();

        playerAC.Player.Look.performed += ctx => cameraLook.Direction = ctx.ReadValue<Vector2>();
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
