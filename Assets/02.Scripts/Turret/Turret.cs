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


    private bool isDying = false; // �ͷ��� �ױ� �������� ����
    private float dyingTimer = 3f; // �ͷ��� �ױ� ���� Ÿ�̸�

    private LineRenderer laserLine; // �������� ��Ÿ���� ���η�����

    void Start()
    {
        laserLine = firePoint.GetComponent<LineRenderer>();
        laserLine.startWidth = 0.01f;  // ������ ũ��
        laserLine.endWidth = 0.03f;
        Material material = new Material(Shader.Find("Standard")); // ������ ����
        material.color = new Color(0, 255, 0, 0f);
    }


    void Update()
    {
        if (!isDying)
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
                    DrawLaser(hit.point); // ������ �׸���
                }
            }
            else
            {
                laserLine.enabled = false; // �÷��̾� ���� ���ϸ� ������x
            }
        }
        else
        {
            dyingTimer -= Time.deltaTime;

            if(dyingTimer <= 0f)
            {
                RotateAndFire(); // �ױ� �������� ���� 
                DrawLaser(target.position);
            }
            else
            {
                StopTurret(); // �ͷ� ����
            }
        }

    }

    void DrawLaser(Vector3 tartgetposition)
    {
        if(laserLine != null)
        {
            laserLine.enabled = true; //������ Ȱ��ȭ

            laserLine.SetPosition(0, firePoint.position); // ������
            laserLine.SetPosition(1, tartgetposition); // Ÿ���� ��ġ
        }
    }

    void RotateAndFire()
    {
        if (isDying) return;

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


    public void StopTurret()
    {
        isDying = false;
        nextFireTime = Mathf.Infinity;
        laserLine.enabled = false; // ������ ��Ȱ��ȭ
    }
}
