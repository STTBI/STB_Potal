using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStateSystem))]
[RequireComponent(typeof(PlayerCameraControl))]
public class PlayerController : MonoBehaviour
{
    // 플레이어 죽음 처리
    public bool IsDeath { get; set; } = false;
    // 플레이어 세이브 포인트
    public Vector3 SavePoint { get; set; } = Vector3.zero;

    // 플레이어 인풋 시스템
    private PlayerInput playerInput;

    // 유니티 컴포넌트
    public Rigidbody Rigid { get; private set; }

    // 스크립트 컴포넌트
    public PlayerMovement Movement { get; private set; }
    public PlayerStateSystem StateSystem { get; private set; }
    public PlayerCameraControl CameraLook { get; private set; }

    // 현재 플레이어가 들고있는 포탈건
    public PortalGun CurrentGun { get; set; }


    // 플레이어 입력 이벤트 관리
    private void Awake()
    {
        // 유니티 컴포넌트
        Rigid = GetComponent<Rigidbody>();

        // 스크립트 컴포넌트
        StateSystem = GetComponent<PlayerStateSystem>();
        Movement = GetComponent<PlayerMovement>();
        CameraLook = GetComponentInChildren<PlayerCameraControl>();
        // 포탈 건이 한개 밖에 없는 이유로 그냥 참조
        CurrentGun = GetComponentInChildren<PortalGun>();


        // 일반 스크립트
        playerInput = new PlayerInput(this);
        // 인풋 시스템 초기화
        playerInput.Initionalize();

        GameManager.Instance.player = this;
    }

    private void OnEnable()
    {
        playerInput.Enable();
        
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
