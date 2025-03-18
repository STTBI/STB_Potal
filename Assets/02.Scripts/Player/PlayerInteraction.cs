using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float checkRate = 0.05f;         //얼마나 자주 ray를 쏴서 체크할 것인지 (0.05초마다 체크)
    private float lastCheckTime;            //마지막으로 체크한 시간
    public float maxCheckDistance = 10f;    //얼마나 멀리 있는 것을 체크할 것인지 (사정거리)
    public LayerMask layerMask;             //레이어 마스크 (어떤 레이어를 체크할 것인지)

    public GameObject interactObject;       //현재 상호작용 하고있는 게임 오브젝트를 받아올 변수
    public IInteractable interactable;      //현재 상호작용 하고있는 오브젝트의 인터페이스를 받아올 변수

    public Camera _camera;                   //화면의 중심

    private void OnValidate()
    {
        layerMask = LayerMask.GetMask("Interactable");
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            //ray를 카메라 중심으로 쏜다.
            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);

            //ray를 맞은 오브젝트 정보를 저장할 변수
            RaycastHit hit;

            //maxCheckDistance 거리 내에서 layerMask에 해당하는 오브젝트만 감지
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                //만약 새로운 오브젝트를 감지했다면
                if (hit.collider.gameObject != interactObject)
                {
                    //감지된 게임 오브젝트 저장
                    interactObject = hit.collider.gameObject;
                    //감지된 게임 오브젝트의 IInteractable 인터페이스 가져오기
                    interactable = hit.collider.GetComponent<IInteractable>();
                }
            }
            else
            {
                //만약 감지된 오브젝트가 없다면
                interactObject = null;
                interactable = null;
            }
        }
    }

    public void OnInteractionInput(InputAction.CallbackContext ctx)
    {
        //키가 눌렸고, 현재 상호작용 가능한 오브젝트를 감지한 상태라면
        if (ctx.phase == InputActionPhase.Started && interactable != null)
        {
            //해당 오브젝트에 해당하는 상호작용 실행
            interactable.OnInteract();
        }
    }
}
