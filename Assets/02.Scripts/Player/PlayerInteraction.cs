using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float checkRate = 0.05f;         //�󸶳� ���� ray�� ���� üũ�� ������ (0.05�ʸ��� üũ)
    private float lastCheckTime;            //���������� üũ�� �ð�
    public float maxCheckDistance = 10f;    //�󸶳� �ָ� �ִ� ���� üũ�� ������ (�����Ÿ�)
    public LayerMask layerMask;             //���̾� ����ũ (� ���̾ üũ�� ������)

    public GameObject interactObject;       //���� ��ȣ�ۿ� �ϰ��ִ� ���� ������Ʈ�� �޾ƿ� ����
    public IInteractable interactable;      //���� ��ȣ�ۿ� �ϰ��ִ� ������Ʈ�� �������̽��� �޾ƿ� ����

    public Camera _camera;                   //ȭ���� �߽�

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

            //ray�� ī�޶� �߽����� ���.
            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            Debug.DrawRay(ray.origin, ray.direction * maxCheckDistance, Color.red);

            //ray�� ���� ������Ʈ ������ ������ ����
            RaycastHit hit;

            //maxCheckDistance �Ÿ� ������ layerMask�� �ش��ϴ� ������Ʈ�� ����
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                //���� ���ο� ������Ʈ�� �����ߴٸ�
                if (hit.collider.gameObject != interactObject)
                {
                    //������ ���� ������Ʈ ����
                    interactObject = hit.collider.gameObject;
                    //������ ���� ������Ʈ�� IInteractable �������̽� ��������
                    interactable = hit.collider.GetComponent<IInteractable>();
                }
            }
            else
            {
                //���� ������ ������Ʈ�� ���ٸ�
                interactObject = null;
                interactable = null;
            }
        }
    }

    public void OnInteractionInput(InputAction.CallbackContext ctx)
    {
        //Ű�� ���Ȱ�, ���� ��ȣ�ۿ� ������ ������Ʈ�� ������ ���¶��
        if (ctx.phase == InputActionPhase.Started && interactable != null)
        {
            //�ش� ������Ʈ�� �ش��ϴ� ��ȣ�ۿ� ����
            interactable.OnInteract();
        }
    }
}
