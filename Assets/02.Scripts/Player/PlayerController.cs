using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MovementHandler))]
[RequireComponent(typeof(PlayerStateSystem))]
public class PlayerController : MonoBehaviour
{
    // 플레이어 인풋 시스템
    public Player_AC PlayerAC { get; private set; }

    // 유니티 컴포넌트
    public Rigidbody Rigid { get; private set; }

    // 스크립트 컴포넌트
    public MovementHandler Movement { get; private set; }
    public PlayerStateSystem StateSystem { get; private set; }

    private void OnValidate()
    {
        // 유니티 컴포넌트
        Rigid = GetComponent<Rigidbody>();

        // 스크립트 컴포넌트
        StateSystem = GetComponent<PlayerStateSystem>();
        Movement = GetComponent<MovementHandler>();
    }
}
