using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    public float rayDistance = 5f;
    public float handleDistance = 2f;
    public float handleSpeed = 2f;
    private GameObject handleObj;

    private Ray ray;

    void Update()
    {
        RaycastHit hit;

        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.CompareTag("Handle"))
            {
                // E키를 누르면 오브젝트를 잡거나 놓음
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (handleObj == null)
                    {
                        // 오브젝트를 잡을 때
                        handleObj = hit.collider.gameObject;
                        Rigidbody rb = handleObj.GetComponent<Rigidbody>();
                        rb.useGravity = false;  // 중력을 끄고
                    }
                    else
                    {
                        // 이미 잡은 오브젝트가 있으면 놓기
                        ReleaseObject();
                    }
                }
                if(handleObj != null)
                {
                    PortalableObject portalableObject = handleObj.GetComponent<PortalableObject>();
                    if(portalableObject != null && portalableObject.IsInPortal)
                    {
                        ReleaseObject();
                    }
                }
            }
        }

        // 오브젝트를 잡고 있으면 위치를 계속 따라오게 한다
        if (handleObj != null)
        {
            GrabObject();
        }
    }

    private void GrabObject()
    {
        // 오브젝트를 잡고 있을 때, 이동 처리
        if (handleObj != null)
        {
            handleObj.transform.position = Vector3.Lerp(handleObj.transform.position, ray.GetPoint(handleDistance), Time.deltaTime * handleSpeed);
        }
    }

    // 오브젝트 놓기 처리
    private void ReleaseObject()
    {
        if (handleObj != null)
        {
            Rigidbody rb = handleObj.GetComponent<Rigidbody>();
            rb.useGravity = true;  // 중력을 다시 켬
            handleObj = null;  // 오브젝트를 놓음
        }
    }
}
