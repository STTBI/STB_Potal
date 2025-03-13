using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private Player_AC playerAC;

    private void Awake()
    {
        playerAC = new Player_AC();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        if(playerAC != null) playerAC.Enable();
    }

    private void OnDisable()
    {
        if(playerAC != null) playerAC.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerAC.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerAC.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame()
    {
        return playerAC.Player.Jump.triggered;
    }
}
