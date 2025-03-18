using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.UIElements;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    [TagSelector] public string selectedTag;
    public float rayDistance = 5f;
    public float handleDistance = 2f;
    public float handleSpeed = 2f;
    private GameObject handleObj;

    public bool isWalkInPortal = false;

    public bool isGrab = false;
    

    private Ray ray;

    void Update()
    {
        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("Portal"); // "PortalLayer"를 제외한 모든 레이어 감지

        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, rayDistance, layerMask)) // 특정 레이어 무시
        {
            if (hit.collider.CompareTag(selectedTag))
            {
                // E키를 누르면 오브젝트를 잡거나 놓음
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (handleObj == null)
                    {
                        handleObj = hit.collider.gameObject;
                        Rigidbody rb = handleObj.GetComponent<Rigidbody>();
                        rb.useGravity = false;
                        handleObj.transform.SetParent(this.transform);  

                        StartCoroutine(GrabDelay(1f));
                    }
                    else
                    {
                        PortalableObject portalableObject = handleObj.GetComponent<PortalableObject>();
                        BoxCollider cd = handleObj.GetComponent<BoxCollider>();
                        cd.isTrigger = false;

                        Debug.Log($"[Update] {handleObj.name} 놓기 시도");

                        handleObj.transform.SetParent(null);
                        portalableObject.enabled = true;
                        
                        ReleaseObject();
                    }
                }

                if (handleObj != null)
                {
                    PortalableObject portalableObject = handleObj.GetComponent<PortalableObject>();
                    BoxCollider cd = handleObj.GetComponent<BoxCollider>();
                    
                    if (isGrab && portalableObject != null && portalableObject.IsInPortal)
                    {
                        portalableObject.enabled = false;
                        cd.isTrigger = true;
                    }
                    else if (!isWalkInPortal && !portalableObject.IsInPortal)
                    {
                        // IsInPortal이 false가 되면 다시 PortalableObject와 BoxCollider를 활성화
                        portalableObject.enabled = true;
                        cd.isTrigger = false;
                    }
                }
            }
        }

        if (handleObj != null)
        {
            GrabObject();
        }
    }

    IEnumerator GrabDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isGrab = true;
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
            isGrab = false;
            Rigidbody rb = handleObj.GetComponent<Rigidbody>();
            rb.useGravity = true;  // 중력을 다시 활성화
            handleObj.transform.SetParent(null);  // 부모 해제
            handleObj = null;
        }
    }
}
