using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;

public class PortalCamera : MonoBehaviour
{
    [SerializeField]
    private Portal[] portals = new Portal[2];  // 포탈 객체 배열, 두 개의 포탈을 사용

    [SerializeField]
    private Camera portalCamera;  // 포탈을 보여줄 카메라

    [SerializeField]
    private int iterations = 7;  // 카메라 렌더링 반복 횟수

    private RenderTexture tempTexture1;  // 첫 번째 포탈 렌더링용 임시 텍스처
    private RenderTexture tempTexture2;  // 두 번째 포탈 렌더링용 임시 텍스처

    private Camera mainCamera;  // 메인 카메라 (현재 카메라)

    private void Awake()
    {
        // 컴포넌트 초기화
        mainCamera = GetComponent<Camera>();

        // 화면 크기와 포맷에 맞게 렌더 텍스처 생성
        tempTexture1 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        tempTexture2 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
    }

    private void Start()
    {
        // 포탈의 렌더러의 재질에 텍스처 할당
        portals[0].Renderer.material.mainTexture = tempTexture1;
        portals[1].Renderer.material.mainTexture = tempTexture2;
    }

    private void OnEnable()
    {
        // 카메라 렌더링 시작 시 UpdateCamera 메서드를 호출하도록 이벤트 등록
        RenderPipeline.beginCameraRendering += UpdateCamera;
    }

    private void OnDisable()
    {
        // 카메라 렌더링 시작 시 UpdateCamera 메서드 호출을 제거하는 이벤트 해제
        RenderPipeline.beginCameraRendering -= UpdateCamera;
    }

    void UpdateCamera(ScriptableRenderContext SRC, Camera camera)
    {
        // 포탈이 배치되지 않았다면 렌더링을 수행하지 않음
        if (!portals[0].IsPlaced || !portals[1].IsPlaced)
        {
            return;
        }

        // 첫 번째 포탈이 화면에 보이면 렌더링
        if (portals[0].Renderer.isVisible)
        {
            portalCamera.targetTexture = tempTexture1;  // 첫 번째 포탈 렌더링용 텍스처 설정
            for (int i = iterations - 1; i >= 0; --i)
            {
                // 첫 번째 포탈을 통해 두 번째 포탈을 렌더링
                RenderCamera(portals[0], portals[1], i, SRC);
            }
        }

        // 두 번째 포탈이 화면에 보이면 렌더링
        if(portals[1].Renderer.isVisible)
        {
            portalCamera.targetTexture = tempTexture2;  // 두 번째 포탈 렌더링용 텍스처 설정
            for (int i = iterations - 1; i >= 0; --i)
            {
                // 두 번째 포탈을 통해 첫 번째 포탈을 렌더링
                RenderCamera(portals[1], portals[0], i, SRC);
            }
        }
    }

    private void RenderCamera(Portal inPortal, Portal outPortal, int iterationID, ScriptableRenderContext SRC)
    {
        // 포탈의 위치와 회전 정보를 얻기 위한 변수들
        Transform inTransform = inPortal.transform;
        Transform outTransform = outPortal.transform;

        Transform cameraTransform = portalCamera.transform;
        cameraTransform.position = transform.position;  // 카메라의 위치를 현재 객체 위치로 설정
        cameraTransform.rotation = transform.rotation;  // 카메라의 회전을 현재 객체 회전으로 설정

        // 주어진 반복 횟수에 대해 카메라 위치 및 회전을 계산하여 포탈을 통과하도록 설정
        for(int i = 0; i <= iterationID; ++i)
        {
            // 카메라를 다른 포탈 뒤쪽에 배치 (반전된 상대 좌표계 사용)
            Vector3 relativePos = inTransform.InverseTransformPoint(cameraTransform.position);
            relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;  // Y축 기준으로 180도 회전
            cameraTransform.position = outTransform.TransformPoint(relativePos);  // 새로운 위치로 설정

            // 카메라가 다른 포탈을 통해 바라보도록 회전 계산
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * cameraTransform.rotation;
            relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot;  // Y축 기준으로 180도 회전
            cameraTransform.rotation = outTransform.rotation * relativeRot;  // 새로운 회전으로 설정
        }

        // 포탈을 통해 바라볼 때의 기울어진 시야(clip plane) 설정
        Plane p = new Plane(-outTransform.forward, outTransform.position);  // 포탈 앞면을 기준으로 평면 생성
        Vector4 clipPlaneWorldSpace = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);  // 월드 공간에서의 clip plane 정보
        Vector4 clipPlaneCameraSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlaneWorldSpace;  // 카메라 공간으로 변환

        // 새로운 프로젝션 매트릭스를 계산하여 카메라에 적용
        var newMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCamera.projectionMatrix = newMatrix;

        // 포탈 카메라 렌더링 수행
        UniversalRenderPipeline.RenderSingleCamera(SRC, portalCamera);
    }
}
