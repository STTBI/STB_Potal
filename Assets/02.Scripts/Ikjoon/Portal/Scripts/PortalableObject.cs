using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PortalableObject : MonoBehaviour
{
    // 클론 객체를 저장할 변수. 포탈을 통해 객체를 복사하여 이동하는 데 사용됨.
    private GameObject cloneObject;

    // 포탈을 통해 이동할 때, 포탈 내부에 있는 객체의 카운트를 추적.
    private int inPortalCount = 0;

    // 입력 포탈 (객체가 들어가는 포탈)
    private Portal inPortal;
    // 출력 포탈 (객체가 나오는 포탈)
    private Portal outPortal;

    // 물리적 특성을 위한 Rigidbody와 Collider 변수.
    private new Rigidbody rigidbody;
    protected new Collider collider;

    public bool IsInPortal = false;

    // 포탈 회전 각도를 180도 반전시키는 고정된 Quaternion. 
    // 포탈의 시점에서 이동할 때 물체의 회전을 반전시킴.
    private static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);

    

    // 초기화: 포탈 객체의 복제본 생성 및 설정
    protected virtual void Awake()
    {
        cloneObject = new GameObject();
        cloneObject.SetActive(false);  // 클론 객체는 기본적으로 비활성화 상태로 생성됨.

        var meshFilter = cloneObject.AddComponent<MeshFilter>();
        var meshRenderer = cloneObject.AddComponent<MeshRenderer>();

        // 클론 객체에 원본 객체의 메쉬와 머티리얼을 복사
        meshFilter.mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer.materials = GetComponent<MeshRenderer>().materials;

        // 클론 객체의 스케일을 원본 객체와 동일하게 설정
        cloneObject.transform.localScale = transform.localScale;

        // 물리적 특성 초기화
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    // LateUpdate: 포탈을 통해 이동할 때, 클론 객체를 이동시키고 회전
    private void LateUpdate()
    {
        if (inPortal == null || outPortal == null)
        {
            return; // 포탈이 설정되지 않은 경우 업데이트하지 않음.
        }

        // 클론 객체가 활성화되어 있고, 두 포탈이 모두 배치되었을 경우
        if (cloneObject.activeSelf && inPortal.IsPlaced && outPortal.IsPlaced)
        {
            var inTransform = inPortal.transform;
            var outTransform = outPortal.transform;

            // 포탈 내부에 있을 때, 클론 객체의 위치 갱신
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            relativePos = halfTurn * relativePos;  // 180도 회전 적용
            cloneObject.transform.position = outTransform.TransformPoint(relativePos);

            // 클론 객체의 회전 갱신
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
            relativeRot = halfTurn * relativeRot;  // 180도 회전 적용
            cloneObject.transform.rotation = outTransform.rotation * relativeRot;
        }
        else
        {
            // 클론 객체가 활성화되지 않으면 위치를 비활성화된 위치로 이동시켜서 숨김.
            cloneObject.transform.position = new Vector3(-1000.0f, 1000.0f, -1000.0f);
        }
    }

    // 포탈에 들어갔을 때 호출되는 메서드
    public void SetIsInPortal(Portal inPortal, Portal outPortal, Collider wallCollider)
    {
        this.inPortal = inPortal;  // 입력 포탈
        this.outPortal = outPortal;  // 출력 포탈
        //IsInPortal = true;

        // 포탈 벽과 충돌하지 않도록 처리
        Physics.IgnoreCollision(collider, wallCollider);

        // 클론 객체 비활성화
        cloneObject.SetActive(false);

        ++inPortalCount;  // 포탈에 들어간 객체의 수 증가
    }

    // 포탈에서 나갔을 때 호출되는 메서드
    public void ExitPortal(Collider wallCollider)
    {
        // 포탈 벽과 충돌 처리를 복원
        Physics.IgnoreCollision(collider, wallCollider, false);
        //IsInPortal = false;
        --inPortalCount;  // 포탈을 나간 객체의 수 감소

        // 포탈 내부에 남아있는 객체가 없으면 클론 객체를 비활성화
        if (inPortalCount == 0)
        {
            Invoke(nameof(DisableCloneObject), 0.5f);
        }
        //collider.isTrigger = false;
    }

    private void DisableCloneObject()
    {
        cloneObject.SetActive(false);
    }

    // 포탈을 통한 이동
    public virtual void Warp()
    {
        // // 워프 쿨타임 체크
        // if (Time.time - lastWarpTime < warpCooldown)
        // {
        //     return; // 0.4초 이내로 다시 워프하지 않음
        // }

        var inTransform = inPortal.transform;
        var outTransform = outPortal.transform;

        // 객체의 위치를 포탈을 통해 이동
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = halfTurn * relativePos;  // 180도 회전 적용
        transform.position = outTransform.TransformPoint(relativePos);

        // 객체의 회전도 포탈을 통해 이동
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        relativeRot = halfTurn * relativeRot;  // 180도 회전 적용
        transform.rotation = outTransform.rotation * relativeRot;

        // 물리적 특성(속도)도 포탈을 통해 이동
        Vector3 relativeVel = inTransform.InverseTransformDirection(rigidbody.velocity);
        relativeVel = halfTurn * relativeVel;  // 180도 회전 적용
        rigidbody.velocity = outTransform.TransformDirection(relativeVel);

        // 포탈 교체
        var tmp = inPortal;
        inPortal = outPortal;
        outPortal = tmp;

       
    }
}
