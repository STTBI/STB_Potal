using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCtrl))]  // CameraMove 대신 PlayerCtrl을 요구합니다.
public class PortalPlacement : MonoBehaviour
{
    [SerializeField]
    private PortalPair portals;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Crosshair crosshair;

    private PlayerCtrl playerCtrl;  // PlayerCtrl 컴포넌트 참조

    private void Awake()
    {
        playerCtrl = GetComponent<PlayerCtrl>();  // PlayerCtrl 컴포넌트 참조
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
            // 포탈이 있는지 확인하고, 있다면 포탈을 통과
            if (hit.collider.tag == "Portal")
            {
                var inPortal = hit.collider.GetComponent<Portal>();

                if (inPortal == null)
                {
                    return;
                }

                var outPortal = inPortal.OtherPortal;

                // 레이캐스트 원점 위치 업데이트
                Vector3 relativePos = inPortal.transform.InverseTransformPoint(hit.point + dir);
                relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
                pos = outPortal.transform.TransformPoint(relativePos);

                // 레이캐스트 방향 업데이트
                Vector3 relativeDir = inPortal.transform.InverseTransformDirection(dir);
                relativeDir = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeDir;
                dir = outPortal.transform.TransformDirection(relativeDir);

                distance -= Vector3.Distance(pos, hit.point);

                FirePortal(portalID, pos, dir, distance);

                return;
            }

            // 포탈 방향 설정
            var cameraRotation = transform.rotation;  // 카메라 회전 값
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

            // 포탈 배치 시도
            bool wasPlaced = portals.Portals[portalID].PlacePortal(hit.collider, hit.point, portalRotation);

            if (wasPlaced)
            {
                crosshair.SetPortalPlaced(portalID, true);
            }
        }
    }
}
