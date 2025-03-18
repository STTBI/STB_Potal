using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed = 5f; // źȯ �ӵ�
    public float lifespawn = 5f; // źȯ ����

    private void OnEnable()
    {
        Invoke(nameof(UnActive), lifespawn);
    }

    void UnActive() => gameObject.SetActive(false);

    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime); //źȯ�� �������� ��� �̵�
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            UnActive(); // �÷��̾�� �浹�� źȯ ����
        }
    }
    void OnDisable()
    {
        ObjectPool.ReturnToPool(gameObject);
        CancelInvoke();
    }


}
