using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;  // �÷��̾�
    public float detectionRange = 10f;  // ���� ����
    public float rotationSpeed = 5f;  // ȸ�� �ӵ�
    public GameObject bulletPrefab;  // �߻��� źȯ ������
    public Transform firePoint;  // źȯ �߻� ��ġ
    public float fireRate = 1f;  // �߻� ����
    private float nextFireTime = 0f;



    void Update()
    {
        // �÷��̾ ������ �������� ��� �κ�
        RaycastHit hit;
        Vector3 direction = target.position - transform.position;

        // �������� �÷��̾ ���� ���
        if (Physics.Raycast(transform.position, direction.normalized, out hit, detectionRange))
        {
            // �������� �÷��̾�� �浹�ϸ�
            if (hit.collider.transform == target)
            {
                // �÷��̾ ������ ���
                RotateAndFire();
            }
        }
        // ������ �ð������� �׸��� (����׿�)
        Debug.DrawRay(transform.position, direction.normalized * detectionRange, Color.red);
    }

    void RotateAndFire()
    {
        // Ÿ���� ���� ȸ��
        Vector3 direction = target.position - transform.position;
        direction.y = 0;  // y�� ȸ�� ���� (���� ȸ���� �ϰ� ��)

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // �߻� Ÿ�̹� Ȯ��
        if (Time.time > nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    
    void FireBullet()// źȯ �߻�
    {
        // �߻�� źȯ�� ����
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
