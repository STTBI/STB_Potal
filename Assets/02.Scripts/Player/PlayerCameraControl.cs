using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCameraControl : MonoBehaviour
{
    private Transform cam;

    [SerializeField] private float cameraSpeed;
    [SerializeField] private float clampAngle;

    public Quaternion TargetRotation { private set; get; }

    public Vector2 Direction { get; set; }

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    private void LateUpdate()
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

    }

    public void ResetTargetRotation()
    {
        TargetRotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
