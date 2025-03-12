using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
  public float moveSpeed = 5f; // 이동 속도
    public float lookSpeedX = 2f; // 마우스 X 회전 속도
    public float lookSpeedY = 2f; // 마우스 Y 회전 속도
    public float jumpForce = 8f; // 점프력
    public float gravity = -9.81f; // 중력

    private float rotationX = 0f;
    private CharacterController characterController;
    private Vector3 velocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 잠금
        characterController = GetComponent<CharacterController>(); // 캐릭터 컨트롤러 컴포넌트
    }

    void Update()
    {
        // 마우스 이동에 따른 시점 변경
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;
        
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // 키보드 입력에 따른 이동
        float moveDirectionY = velocity.y; // Y 방향 속도 보존 (점프와 중력 반영)
        velocity = (transform.TransformDirection(Vector3.forward) * Input.GetAxis("Vertical") +
                    transform.TransformDirection(Vector3.right) * Input.GetAxis("Horizontal"));
        velocity.y = moveDirectionY; // Y 방향 속도 복구

        // 점프 구현
        if (characterController.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                velocity.y = jumpForce; // 점프시 Y 방향에 힘을 추가
            }
            else
            {
                velocity.y = -2f; // 살짝 하강하도록 설정 (바닥에 있을 때 중력 느낌)
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // 중력 적용
        }

        // 캐릭터 이동
        characterController.Move(velocity * moveSpeed * Time.deltaTime);
    }
}