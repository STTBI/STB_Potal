using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]  // CapsuleCollider로 변경
public class Portal : MonoBehaviour
{
    [field: SerializeField]
    public Portal OtherPortal { get; private set; }

    [SerializeField]
    private Renderer outlineRenderer;
    public Renderer portalRenderer;

    [field: SerializeField]
    public Color PortalColour { get; private set; }

    [SerializeField]
    private LayerMask placementMask;

    [SerializeField]
    private Transform testTransform;

    private List<PortalableObject> portalObjects = new List<PortalableObject>();
    public bool IsPlaced { get; private set; } = false;
    private Collider wallCollider;

    // 컴포넌트들.
    public Renderer Renderer { get; private set; }
    private new CapsuleCollider collider;  // CapsuleCollider로 변경

    private void Awake()
    {
        // CapsuleCollider와 Renderer 컴포넌트를 가져옴.
        collider = GetComponent<CapsuleCollider>();
        Renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        // 아웃라인 컬러를 포탈 컬러로 설정.
        outlineRenderer.material.SetColor("_OutlineColour", PortalColour);
        
        // 포탈을 비활성화 상태로 설정.
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // 다른 포탈이 배치되어 있으면 렌더러 활성화.
        Renderer.enabled = OtherPortal.IsPlaced;

        // 포탈에 들어간 모든 물체들에 대해 Warp 실행.
        for (int i = 0; i < portalObjects.Count; ++i)
        {
            Vector3 objPos = transform.InverseTransformPoint(portalObjects[i].transform.position);

            // 포탈에 들어간 물체의 z축 위치가 0보다 크면 포탈을 통과.
            if (objPos.z > 0.0f)
            {
                portalObjects[i].Warp();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 포탈에 들어온 물체 처리.
        var obj = other.GetComponent<PortalableObject>();
        if (obj != null)
        {
            portalObjects.Add(obj);
            obj.SetIsInPortal(this, OtherPortal, wallCollider);
            
            if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                CameraMove cameraMove = other.GetComponent<CameraMove>();
                if(cameraMove.currentVelocity.magnitude > 10f)
                {
                    cameraMove.isInPortal = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 포탈을 나가는 물체 처리.
        var obj = other.GetComponent<PortalableObject>();

        if(portalObjects.Contains(obj))
        {
            portalObjects.Remove(obj);
            obj.ExitPortal(wallCollider);
        }
    }

    // 포탈을 배치할 수 있는지 확인 후, 배치가 가능하면 포탈을 배치.
    public bool PlacePortal(Collider wallCollider, Vector3 pos, Quaternion rot)
    {
        testTransform.position = pos;
        testTransform.rotation = rot;
        testTransform.position -= testTransform.forward * 0.001f;

        // 포탈 배치 전의 충돌을 수정.
        FixOverhangs();
        FixIntersects();

        // 겹침이 없으면 포탈을 배치.
        if (CheckOverlap())
        {
            this.wallCollider = wallCollider;
            transform.position = testTransform.position;
            transform.rotation = testTransform.rotation;

            // 포탈을 활성화하고 배치 완료.
            gameObject.SetActive(true);
            IsPlaced = true;

            portalRenderer.transform.localScale = Vector3.zero;
            StartCoroutine(GrowPortal());

            return true;
        }

        return false;
    }

    private IEnumerator GrowPortal()
    {
        float duration = 0.5f;
        float elapsed =0f;

        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            portalRenderer.transform.localScale = Vector3.Lerp(startScale, targetScale, progress);
            yield return null;
        }
        portalRenderer.transform.localScale = targetScale;
    }

    // 포탈이 겹치지 않도록 하기 위해 포탈이 벽을 넘어가지 않도록 수정.
    private void FixOverhangs()
    {
        var testPoints = new List<Vector3>
        {
            new Vector3(-1.1f,  0.0f, 0.1f),
            new Vector3( 1.1f,  0.0f, 0.1f),
            new Vector3( 0.0f, -2.1f, 0.1f),
            new Vector3( 0.0f,  2.1f, 0.1f)
        };

        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        for(int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = testTransform.TransformPoint(testPoints[i]);
            Vector3 raycastDir = testTransform.TransformDirection(testDirs[i]);

            if(Physics.CheckSphere(raycastPos, 0.05f, placementMask))
            {
                break;
            }
            else if(Physics.Raycast(raycastPos, raycastDir, out hit, 2.1f, placementMask))
            {
                var offset = hit.point - raycastPos;
                testTransform.Translate(offset, Space.World);
            }
        }
    }

    // 포탈이 벽과 겹치지 않도록 하기 위해 충돌을 수정.
    private void FixIntersects()
    {
        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        var testDists = new List<float> { 1.1f, 1.1f, 2.1f, 2.1f };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = testTransform.TransformPoint(0.0f, 0.0f, -0.1f);
            Vector3 raycastDir = testTransform.TransformDirection(testDirs[i]);

            if (Physics.Raycast(raycastPos, raycastDir, out hit, testDists[i], placementMask))
            {
                var offset = (hit.point - raycastPos);
                var newOffset = -raycastDir * (testDists[i] - offset.magnitude);
                testTransform.Translate(newOffset, Space.World);
            }
        }
    }

    // 포탈이 다른 물체와 겹치지 않는지 확인.
    private bool CheckOverlap()
    {
        var checkExtents = new Vector3(0.9f, 1.9f, 0.05f);

        var checkPositions = new Vector3[]
        {
            testTransform.position + testTransform.TransformVector(new Vector3( 0.0f,  0.0f, -0.1f)),

            testTransform.position + testTransform.TransformVector(new Vector3(-1.0f, -2.0f, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3(-1.0f,  2.0f, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3( 1.0f, -2.0f, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3( 1.0f,  2.0f, -0.1f)),

            testTransform.TransformVector(new Vector3(0.0f, 0.0f, 0.2f))
        };

        // 포탈이 벽과 겹치지 않는지 확인.
        var intersections = Physics.OverlapCapsule(checkPositions[0], checkPositions[1], 0.05f, placementMask); // OverlapBox에서 OverlapCapsule로 변경

        if(intersections.Length > 1)
        {
            return false;
        }
        else if(intersections.Length == 1) 
        {
            // 이전 포탈 위치와 겹치는 것은 허용.
            if (intersections[0] != collider)
            {
                return false;
            }
        }

        return true;
    }

    // 포탈을 제거.
    public void RemovePortal()
    {
        gameObject.SetActive(false);
        IsPlaced = false;
    }
}
