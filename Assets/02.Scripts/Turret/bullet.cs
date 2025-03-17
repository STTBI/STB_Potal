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
}
