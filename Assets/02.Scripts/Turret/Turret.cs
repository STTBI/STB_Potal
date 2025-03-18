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

    public float maxRotationAngle = 30f; // �ͷ��� ȸ���� �� �ִ� �ִ� ����

    private bool isLifted = false; // �÷��̾ �ͷ��� ������� ����
    private Vector3 liftOffset = new Vector3(0, 3f, 0); // �ͷ��� �÷��̾�� �鸱 ��

    void Start()
    {
        laserLine = firePoint.GetComponent<LineRenderer>();
        laserLine.startWidth = 0.01f;  // ������ ũ��
        laserLine.endWidth = 0.02f;
        laserLine.material = new Material(Shader.Find("Sprites/Default")); // ������ ����
        laserLine.startColor = Color.white;
        laserLine.endColor = Color.red;
    }


    void Update()
    {
        // �ͷ��� ��� ���
        if (Input.GetKeyDown(KeyCode.C))
        {
            LiftTurretByPlayer();
        }

        //  �ͷ��� ���� ���
        if (Input.GetKeyDown(KeyCode.X))
        {
            DropTurret();
        }

        if (isLifted)
        {
            // �÷��̾� ��ġ�� ���� �ͷ��� �̵�
            LiftTurret();
        }
        else
        {
            if (!isDying)
            {
                // �÷��̾ ������ �������� ��� �κ�
                RaycastHit hit;
                Vector3 direction = target.position - transform.position;

                // ���鿡���� �����ϰ� ���� ����
                float angle = Vector3.Angle(transform.forward, direction);
                if (angle <= maxRotationAngle)
                {
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
                    laserLine.enabled = false; // ������ ���� �ܿ� �÷��̾� ����x
                }
            }
            else
            {
                dyingTimer -= Time.deltaTime;

                if (dyingTimer <= 0f)
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
    }

    void LiftTurret()
    {
        if (target != null)
        {
            // �÷��̾�� �ͷ��� �鸰 ���¿��� �÷��̾� ��ġ�� ���� �̵�
            transform.position = target.position + liftOffset;
        }
    }

    void DrawLaser(Vector3 tartgetposition)
    {
        if(laserLine != null)
        {
            laserLine.enabled = true; //������ Ȱ��ȭ

            laserLine.positionCount = 2;

            laserLine.SetPosition(0, firePoint.position); // ������
            laserLine.SetPosition(1, tartgetposition); // Ÿ���� ��ġ
        }
    }

    void RotateAndFire()
    {
        if (isDying) return;

        // Ÿ���� ���� ȸ��
        Vector3 direction = target.position - transform.position;
        direction.y = 0;  // y�� ȸ�� ����

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // �߻� Ÿ�̹� Ȯ��
        if (Time.time > nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
   
    void FireBullet() // źȯ �߻�
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

    public void LiftTurretByPlayer()
    {
        isLifted = true; // �ͷ��� �鸰 ����
    }

    public void DropTurret()
    {
        isLifted = false; // �ͷ��� �������� ��
        transform.position = new Vector3(0, 1, 0); // ���� ��ġ�� �ǵ�����
    }

    /*
     �ڵ� �����Ͻǲ�
    
    FireBullet�Լ� �Ʒ��� ���� ObjectPool���� Bullet�� Spawn�ϵ���
    void FireBullet() // źȯ �߻�
    {
        // �߻�� źȯ�� ����
        ObjectPool.SpawnFromPool("Bullet", firePoint.position, firePoint.rotation);
    }

     */

}
