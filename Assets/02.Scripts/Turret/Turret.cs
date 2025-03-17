using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;  // �÷��̾�
    public Transform RobotGun; // ���� �ѱ�
    public Transform RobotMount; // ���� �����
    public Transform UpperBody; // ���� ��ݽ�
    public Transform RigBodyLower; // ���� �Ϲݽ�
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

        // �÷��̾� �±׷� ã��
        GameObject player = GameObject.FindWithTag("Player");

        // SentryRobot���� RobotGun�� ã��, RobotGun�� �ڽ����� �ִ� firePoint�� �Ҵ�
        GameObject sentryRobot = GameObject.Find("SentryRobot");
        if (sentryRobot != null)
        {
            Transform robotGun = sentryRobot.transform.Find("Rig_Body_Upper/RobotGun");
            if (robotGun != null)
            {
                firePoint = robotGun.Find("firePoint");  // RobotGun�� �ڽĿ� �ִ� firePoint�� ã��
                if (firePoint == null)
                {
                    Debug.LogWarning("firePoint not found in RobotGun");
                }
            }
            else
            {
                Debug.LogWarning("RobotGun not found in SentryRobot");
            }
        }
        else
        {
            Debug.LogWarning("SentryRobot not found in the scene");
        }
    }


    void Update()
    {
        RotateTowardsPlayer(); // ��ݽŸ� �÷��̾� ������ ȸ��

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

    void RotateTowardsPlayer()
    {
        if (target != null)
        {
            // �÷��̾��� ��ġ�� ȸ�� ���� ���
            Vector3 direction = target.position - transform.position;
            direction.y = 0;  // y���� �����Ͽ� ���Ʒ� ȸ���� ����

            // ��ǥ ȸ�� ���� ��� (Y�ุ ȸ��)
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // ȸ�� ������ �����ϱ� ���� EulerAngles�� ���
            Vector3 currentRotation = targetRotation.eulerAngles;

            currentRotation.x = -90f;  // X�� -90���� ����

            // ��ǥ ȸ�� ������ ���� ȸ�� ������ ���̸� ���Ͽ� ������ ����
            float angle = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, currentRotation.y));

            if (angle <= maxRotationAngle)
            {
                // ȸ�� ���� �̳��� ��� �ε巴�� ȸ��
                RobotGun.rotation = Quaternion.Slerp(RobotGun.rotation, Quaternion.Euler(currentRotation), rotationSpeed * Time.deltaTime);
                RobotMount.rotation = Quaternion.Slerp(RobotMount.rotation, Quaternion.Euler(currentRotation), rotationSpeed * Time.deltaTime);
                UpperBody.rotation = Quaternion.Slerp(UpperBody.rotation, Quaternion.Euler(currentRotation), rotationSpeed * Time.deltaTime);
            }
            else
            {
                // ȸ�� ������ �ʰ��ϸ� ȸ������ ����
                Debug.Log("Rotation restricted");
            }
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
}
