using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    Player_AC playerAC;
    PlayerMovement movement;

    public PlayerInput(PlayerController player)
    {
        playerAC = new Player_AC();
        movement = player.Movement;
    }

    public void Initionalize()
    {
        playerAC.Player.Movement.performed += ctx => movement.Direction = ctx.ReadValue<Vector2>();
        playerAC.Player.Movement.canceled += ctx => movement.Direction = Vector2.zero;
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
