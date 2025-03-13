using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStateSystem))]
public class PlayerController : MonoBehaviour
{
    public PlayerMovement MoveMent { get; set; }
    public PlayerStateSystem StateSystem { get; set; }
    public Animator Anim { get; set; }

    private void OnValidate()
    {
        MoveMent = GetComponent<PlayerMovement>();
        StateSystem = GetComponent<PlayerStateSystem>();
        Anim = GetComponentInChildren<Animator>();
    }
}
