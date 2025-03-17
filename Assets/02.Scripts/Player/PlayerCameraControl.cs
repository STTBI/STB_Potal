using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCameraControl : MonoBehaviour
{
    private Transform cam;

    [SerializeField] private float sensivityX;
    [SerializeField] private float sensivityY;

    private Vector2 mouseSensivity;
    [SerializeField] private float clampAngle;

    public Quaternion TargetRotation { private set; get; }


    public Vector2 Direction { get; set; }

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    private void LateUpdate()
    {

        mouseSensivity.x = -Direction.y * sensivityX * Time.deltaTime;
        mouseSensivity.y = Direction.x * sensivityY * Time.deltaTime;

        // 카메라 회전
        if (mouseSensivity.y > 180.0f)
        {
            mouseSensivity.y -= 360.0f;
        }
        mouseSensivity.x = Mathf.Clamp(mouseSensivity.x, -clampAngle, clampAngle);

        Quaternion camRotation = cam.localRotation;
        camRotation = camRotation *  Quaternion.Euler(mouseSensivity.x, 0f, 0f);
        cam.localRotation = camRotation;

        Quaternion playerRotation = transform.rotation;
        playerRotation = playerRotation * Quaternion.Euler(0f, mouseSensivity.y, 0f);
        transform.rotation = playerRotation;

        // 포탈 마우스 동기화
        TargetRotation = Quaternion.Euler((Vector3)mouseSensivity);
    }

    public void ResetTargetRotation()
    {
        TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
