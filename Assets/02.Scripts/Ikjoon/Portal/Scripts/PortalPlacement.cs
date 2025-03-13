using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraMove))]
public class PortalPlacement : MonoBehaviour
{
    [SerializeField]
    private PortalPair portals;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Crosshair crosshair;

    private CameraMove cameraMove;

    private void Awake()
    {
        cameraMove = GetComponent<CameraMove>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FirePortal(0, transform.position, transform.forward, 250.0f);
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            FirePortal(1, transform.position, transform.forward, 250.0f);
        }
    }

    private void FirePortal(int portalID, Vector3 pos, Vector3 dir, float distance)
    {
        RaycastHit hit;
        Physics.Raycast(pos, dir, out hit, distance, layerMask);

        if (hit.collider != null)
        {
            // 포탈에 충돌했을 때, 재귀적으로 포탈을 통해 레이캐스트를 발사
            if (hit.collider.tag == "Portal")
            {
                var inPortal = hit.collider.GetComponent<Portal>();

                if (inPortal == null)
                {
                    return;
                }

                var outPortal = inPortal.OtherPortal;

                // 레이캐스트의 원점을 다른 포탈의 위치에 맞게 계산
                Vector3 relativePos = inPortal.transform.InverseTransformPoint(hit.point + dir);
                relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
                pos = outPortal.transform.TransformPoint(relativePos);

                // 레이캐스트의 방향도 포탈을 통과하도록 계산
                Vector3 relativeDir = inPortal.transform.InverseTransformDirection(dir);
                relativeDir = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeDir;
                dir = outPortal.transform.TransformDirection(relativeDir);

                distance -= Vector3.Distance(pos, hit.point);

                // 재귀적으로 포탈을 통해 레이캐스트 발사
                FirePortal(portalID, pos, dir, distance);

                return;
            }

            // 레이캐스트가 포탈이 아니면, 충돌한 표면에 포탈을 배치
            var cameraRotation = cameraMove.TargetRotation;
            var portalRight = cameraRotation * Vector3.right;
            
            if (Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))
            {
                portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;
            }
            else
            {
                portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;
            }

            var portalForward = -hit.normal;
            var portalUp = -Vector3.Cross(portalRight, portalForward);

            var portalRotation = Quaternion.LookRotation(portalForward, portalUp);
            
            // 포탈을 배치하려 시도
            bool wasPlaced = portals.Portals[portalID].PlacePortal(hit.collider, hit.point, portalRotation);

            if (wasPlaced)
            {
                crosshair.SetPortalPlaced(portalID, true);
            }
        }
    }

    // 레이캐스트 경로를 Gizmos로 시각화
    private void OnDrawGizmos()
    {
        // 레이캐스트의 시작 위치와 방향을 가져와서 레이그리기
        Vector3 startPos = transform.position;
        Vector3 direction = transform.forward * 250.0f;  // 레이캐스트 거리

        Gizmos.color = Color.red;  // 레이의 색상 설정
        Gizmos.DrawLine(startPos, startPos + direction);  // 레이캐스트 경로 그리기
    }
}
