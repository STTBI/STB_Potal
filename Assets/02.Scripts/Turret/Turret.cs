using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;  // 플레이어
    public Transform RobotGun; // 에셋 총구
    public Transform RobotMount; // 에셋 연결부
    public Transform UpperBody; // 에셋 상반신
    public Transform RigBodyLower; // 에셋 하반신
    public float detectionRange = 10f;  // 감지 범위
    public float rotationSpeed = 5f;  // 회전 속도
    public GameObject bulletPrefab;  // 발사할 탄환 프리팹
    public Transform firePoint;  // 탄환 발사 위치
    public float fireRate = 1f;  // 발사 간격
    private float nextFireTime = 0f;

    private bool isDying = false; // 터렛이 죽기 직전인지 여부
    private float dyingTimer = 3f; // 터렛이 죽기 직전 타이머

    private LineRenderer laserLine; // 레이저를 나타내는 라인랜더러

    public float maxRotationAngle = 30f; // 터렛이 회전할 수 있는 최대 각도

    private bool isLifted = false; // 플레이어가 터렛을 들었는지 여부
    private Vector3 liftOffset = new Vector3(0, 3f, 0); // 터렛이 플레이어에게 들릴 때

    void Start()
    {
        laserLine = firePoint.GetComponent<LineRenderer>();
        laserLine.startWidth = 0.01f;  // 레이저 크기
        laserLine.endWidth = 0.02f;
        laserLine.material = new Material(Shader.Find("Sprites/Default")); // 레이저 색상
        laserLine.startColor = Color.white;
        laserLine.endColor = Color.red;

        // 플레이어 태그로 찾기
        GameObject player = GameObject.FindWithTag("Player");

        // SentryRobot에서 RobotGun을 찾고, RobotGun의 자식으로 있는 firePoint를 할당
        GameObject sentryRobot = GameObject.Find("SentryRobot");
        if (sentryRobot != null)
        {
            Transform robotGun = sentryRobot.transform.Find("Rig_Body_Upper/RobotGun");
            if (robotGun != null)
            {
                // RobotGun의 자식으로 firePoint가 있으므로, 그 자식 Transform을 firePoint로 설정
                firePoint = robotGun.Find("firePoint");  // RobotGun의 자식에 있는 firePoint를 찾기
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
        RotateTowardsPlayer(); // 상반신만 플레이어 쪽으로 회전

        if (isLifted)
        {
            // 플레이어 위치에 맞춰 터렛을 이동
            LiftTurret();
        }
        else
        {
            if (!isDying)
            {
                // 플레이어를 감지할 레이저를 쏘는 부분
                RaycastHit hit;
                Vector3 GoalPosition = target.position;
                GoalPosition.y = transform.position.y + 0.6f; // y값으로 레이저 높이 조정

                Vector3 direction = GoalPosition - transform.position;

                // 정면에서만 감지하게 각도 조절
                float angle = Vector3.Angle(transform.forward, direction);
                if (angle <= maxRotationAngle)
                {
                    // 플레이어가 감지 범위 내에 있는지 확인
                    float distance = Vector3.Distance(transform.position, GoalPosition);
                    if (distance <= detectionRange) // 감지 범위 내에 플레이어가 있으면
                    {
                        RotateAndFire();
                        DrawLaser(GoalPosition); // 레이저 그리기
                    }
                    else
                    {
                        laserLine.enabled = false; // 플레이어 감지 못하면 레이저x
                    }
                }
                else
                {
                    laserLine.enabled = false; // 정해진 각도 외에 플레이어 감지x
                }
            }
            else
            {
                dyingTimer -= Time.deltaTime;

                if (dyingTimer <= 0f)
                {
                    RotateAndFire(); // 죽기 직전까지 공격 
                    DrawLaser(target.position);
                }
                else
                {
                    StopTurret(); // 터렛 중지
                }
            }
        }
    }

    void LiftTurret()
    {
        if (target != null)
        {
            // 플레이어에게 터렛이 들린 상태에서 플레이어 위치에 따라 이동
            transform.position = target.position + liftOffset;
        }
    }

    void DrawLaser(Vector3 targetPosition)
    {
        if (laserLine != null)
        {
            laserLine.enabled = true; //레이저 활성화

            laserLine.positionCount = 2;

            laserLine.SetPosition(0, firePoint.position); // 시작점
            laserLine.SetPosition(1, targetPosition); // 타겟의 위치
        }
    }

    void RotateAndFire()
    {
        if (isDying) return;

        // 발사 타이밍 확인
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
            // 플레이어의 위치로 회전 방향 계산
            Vector3 direction = target.position - transform.position;
            direction.y = 0;  // Y축을 고정하여 위아래 회전 방지

            // 목표 회전 방향 계산 (Y축만 회전)
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 회전 각도를 제한하기 위해 EulerAngles 사용
            Vector3 currentRotation = targetRotation.eulerAngles;

            // X와 Z축을 고정 (Y축만 회전)
            currentRotation.x = -90f;  // X는 고정

            // 목표 회전 각도와 현재 회전 각도의 차이를 구하여 제한을 적용
            float angle = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, currentRotation.y));

            // 회전 제한
            if (angle <= maxRotationAngle)
            {
                // RobotGun, RobotMount, UpperBody만 회전시키기
                RobotGun.rotation = Quaternion.Slerp(RobotGun.rotation, Quaternion.Euler(currentRotation), rotationSpeed * Time.deltaTime);
                RobotMount.rotation = Quaternion.Slerp(RobotMount.rotation, Quaternion.Euler(currentRotation), rotationSpeed * Time.deltaTime);
                UpperBody.rotation = Quaternion.Slerp(UpperBody.rotation, Quaternion.Euler(currentRotation), rotationSpeed * Time.deltaTime);
            }
            else
            {
                // 회전 제한을 초과하면 회전하지 않음
                Debug.Log("Rotation restricted");
            }
        }
    }

    void FireBullet() // 탄환 발사
    {
        // 발사된 탄환을 생성
        ObjectPool.SpawnFromPool("Bullet", firePoint.position, firePoint.rotation); // ObjectPool에서 Bullet을 Spawn하도록
    }

    public void StopTurret()
    {
        isDying = false;
        nextFireTime = Mathf.Infinity;
        laserLine.enabled = false; // 레이저 비활성화
    }

    public void LiftTurretByPlayer()
    {
        isLifted = true; // 터렛이 들린 상태
    }

    public void DropTurret()
    {
        isLifted = false; // 터렛을 내려놓을 때
        transform.position = new Vector3(0, 1, 0); // 원래 위치로 되돌리기
    }
}
