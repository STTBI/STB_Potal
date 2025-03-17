using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerCameraControl : MonoBehaviour
{
    private Transform cam;

    [SerializeField] private float sensivityX;
    [SerializeField] private float sensivityY;

    private Quaternion playerRotation;
    private Vector3 camRotation;

    private Vector2 mouseSensivity;
    [SerializeField] private float clampAngle;

    public Quaternion TargetRotation { private set; get; }


    public Vector2 Direction { get; set; }

    private void Awake()
    {
        cam = Camera.main.transform;
        playerRotation = transform.rotation;
        camRotation = Vector3.zero;
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

        playerRotation = playerRotation * Quaternion.Euler(0f, mouseSensivity.y, 0f);
        transform.rotation = playerRotation;

        camRotation += Vector3.right * mouseSensivity.x;
        camRotation.x = Mathf.Clamp(camRotation.x, -clampAngle, clampAngle);
        Debug.Log(camRotation);
        cam.localRotation = Quaternion.Euler(camRotation.x, 0, 0);


        // 포탈 마우스 동기화
        TargetRotation = Quaternion.Euler(camRotation.x, playerRotation.y, 0f);
    }

    public void ResetTargetRotation()
    {
        TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
