using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class CameraMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7.5f; // 이동 속도
    private const float cameraSpeed = 3.0f; // 카메라 회전 속도
    public float jumpForce = 40.0f; // 점프 힘
    private const float gravity = 9.81f; // 중력
    public float maxVelocity = -30f;
    public bool isGrounded = false;

    [Header("Raycast")]
    public float rayLength = 2f;

    public Quaternion TargetRotation { private set; get; }

    private Vector3 moveVector = Vector3.zero;
    private float moveY = 0.0f;
    public bool isInPortal = false;
    
    

    private new Rigidbody rigidbody;
    
    [SerializeField]
    public Vector3 currentVelocity;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true; // 중력 적용
        Cursor.lockState = CursorLockMode.Locked;

        TargetRotation = transform.rotation;
    }

    private void Update()
    {
        // 카메라 회전
        var rotation = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        var targetEuler = TargetRotation.eulerAngles + (Vector3)rotation * cameraSpeed;
        if (targetEuler.x > 180.0f)
        {
            targetEuler.x -= 360.0f;
        }
        targetEuler.x = Mathf.Clamp(targetEuler.x, -90.0f, 90.0f);
        TargetRotation = Quaternion.Euler(targetEuler);

        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation,
            Time.deltaTime * 15.0f);

        // 이동
        if(isInPortal == false)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            moveVector = new Vector3(x, 0.0f, z) * moveSpeed;
        }if(isInPortal == true && Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = 0.2f;
        }else
        {
            Time.timeScale = 1f;
        }
        
        //moveY = Input.GetAxis("Elevation");

        // 점프 처리
        if (isGrounded && Input.anyKeyDown)
        {
            isInPortal = false;
            
            if(Input.GetKeyDown(KeyCode.Space)) // 점프 버튼이 눌리면
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z); // 점프
            }
        }
    }

    private void FixedUpdate()
    {
        // Raycast를 캐릭터 발 아래에서 발사하여 바닥에 닿았는지 체크
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, rayLength);

        Vector3 newVelocity;

        if (isInPortal == false)
        {
            // 일반 이동 처리
            newVelocity = transform.TransformDirection(moveVector);
            newVelocity.y = rigidbody.velocity.y; // Y 방향 속도는 물리엔진에 의해 처리
        }
        else
        {
            // 포탈 안에서는 기존 속도를 유지
            newVelocity = rigidbody.velocity;
        }

        // Y축 속도가 maxVelocity를 넘지 않도록 제한
        if (newVelocity.y < maxVelocity)
        {
            newVelocity.y = maxVelocity;
        }

        rigidbody.velocity = newVelocity;
        currentVelocity = newVelocity;
    }


    public void ResetTargetRotation()
    {
        TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }

    // Raycast 경로를 Gizmos로 시각화 (디버그용)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
         Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + Vector3.up * 0.1f + Vector3.down * rayLength);
    }
} 