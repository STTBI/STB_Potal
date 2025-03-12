using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RenderPipeLine = UnityEngine.Rendering.RenderPipelineManager;

public class PortalCamera : MonoBehaviour
{
    [SerializeField]
    private Portal[] portals = new Portal[2];

    [SerializeField]
    private Camera portalCam;

    [SerializeField]
    private int interations = 7;

    private RenderTexture tempTexture1;
    private RenderTexture tempTexture2;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = GetComponent<Camera>();

        tempTexture1 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        tempTexture2 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
    }

    private void Start()
    {
        portals[0].Renderer.material.mainTexture = tempTexture1;
        portals[1].Renderer.material.mainTexture = tempTexture2;
    }

    private void OnEnable()
    {
       RenderPipeLine.beginCameraRendering += UpdateCamera;
    }

    private void OsDisable()
    {
        RenderPipeLine.beginCameraRendering -= UpdateCamera;
    }

    void UpdateCamera(ScriptableRenderContext SRC, Camera camera)
    {
        if(!portals[0].IsPlaced || !portals[1].IsPlaced)
        {
            return;
        }

        if(portals[0].Renderer.isVisible)
        {
            portalCam.targetTexture = tempTexture1;
            for(int i = interations -1; i >= 0; --i)
            {
                RenderCamera(portals[0], portals[1], i, SRC);
            }
        }
        if(portals[1].Renderer.isVisible)
        {
            portalCam.targetTexture = tempTexture2;
            for(int i = interations -1 ; i >= 0; --i)
            {
                RenderCamera(portals[1], portals[0], i ,SRC);
            }
        }
    }
    private void RenderCamera(Portal inPortal, Portal outPortal, int iterationID, ScriptableRenderContext SRC)
    {
        // 입구 포탈과 출구 포탈의 변환 정보 가져오기
        Transform inTransform = inPortal.transform;
        Transform outTransform = outPortal.transform;

        // 포탈 카메라의 변환 정보 가져오기
        Transform cameraTransform = portalCam.transform;
        cameraTransform.position = transform.position; // 카메라 위치를 현재 오브젝트 위치로 설정
        cameraTransform.rotation = transform.rotation; // 카메라 회전을 현재 오브젝트 회전으로 설정

        // 지정된 반복 횟수(iterationID) 만큼 카메라의 위치와 회전을 조정
        for(int i = 0; i <= iterationID; ++i)
        {
            // 다른 포탈 뒤에 카메라를 위치시킴
            Vector3 relativePos = inTransform.InverseTransformPoint(cameraTransform.position);
            relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos; // 180도 회전
            cameraTransform.position = outTransform.TransformPoint(relativePos); // 출구 포탈 기준으로 위치 계산

            // 카메라를 다른 포탈을 통해 바라보도록 회전시킴
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * cameraTransform.rotation;
            relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot; // 180도 회전
            cameraTransform.rotation = outTransform.rotation * relativeRot; // 출구 포탈 기준으로 회전 계산
        }

        // 카메라의 사선 클리핑 평면 설정
        Plane p = new Plane(-outTransform.forward, outTransform.position); // 출구 포탈의 전방 방향으로 평면 생성
        Vector4 clipPlaneWorldSpace = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance); // 월드 공간에서의 평면 정보
        Vector4 clipPlaneCameraSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(portalCam.worldToCameraMatrix)) * clipPlaneWorldSpace; // 카메라 공간으로 변환

        // 새로운 사선 클리핑 행렬 계산
        var newMatrix = mainCam.CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCam.projectionMatrix = newMatrix; // 포탈 카메라의 프로젝션 매트릭스 설정

        // 포탈 카메라를 렌더 타겟으로 렌더링 쉬바 ㅈㄴ어렵네
        UniversalRenderPipeline.RenderSingleCamera(SRC, portalCam);
    }
}
