using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed = 5f; // źȯ �ӵ�
    public float lifespawn = 5f; // źȯ ����

    void Start()
    {
        Destroy(gameObject, lifespawn); //źȯ �߻� �� ���� �ð� ���� �� �ı�
    }

    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime); //źȯ�� �������� ��� �̵�
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject); // �÷��̾�� �浹�� źȯ ����
        }
    }

    /*
    Start�Լ��� OnEnable�� ����

    private void OnEnable()
    {
        Invoke(nameof(UnActive), lifespawn);
    }

    Invoke�� lifespawn�Ŀ� ȣ��� �Լ� �߰� �ڽ��� ��Ȱ��ȭ
    void UnActive() => gameObject.SetActive(false); 

    ��Ȱ��ȭ �ɽ� ������ƮǮ���� ��� Invoke���������ֱ� ���� CancelInvoke
    void OnDisable()
    {
        ObjectPool.ReturnToPool(gameObject);
        CancelInvoke();
    }

    ��ũ��Ʈ���� Destroy(gameObject) ���� UnActive() �� �����ϸ�˴ϴ�.

    ������ �׽�Ʈ �ϽǶ� �߰��Ұ�
    CreateEmpty - ObjectPool ��ũ��Ʈ ������ - Pools���� + ��ư - Prefab�� Bullet ������ ���
    Tag���� �����հ� ������ �̸�, Size���� ������Ʈ Ǯ ũ�� ex)20

     */

}
