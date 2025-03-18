using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Vector3 GoalPosition;
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

    private PlayerController playerObject;

    void Start()
    {
        playerObject = GameManager.Instance.player;
        laserLine = firePoint.GetComponent<LineRenderer>();
        laserLine.startWidth = 0.01f;  // 레이저 크기
        laserLine.endWidth = 0.02f;
        laserLine.material = new Material(Shader.Find("Sprites/Default")); // 레이저 색상
        laserLine.startColor = Color.white;
        laserLine.endColor = Color.red;
    }

    void Update()
    {
        if (playerObject != null)
        {
            Transform target = playerObject.transform;  // 플레이어의 Transform을 동적으로 찾기
            Debug.Log($"playerObject {playerObject.IsDeath}");
            Debug.Log($"Turret {isDying}");
            // 플레이어가 죽지 않았고, 터렛이 죽지 않은 경우
            if (!isDying)  // 플레이어가 죽었는지를 체크
            {
                if (playerObject.IsDeath) return;
                RotateTowardsPlayer(target); // 상반신만 플레이어 쪽으로 회전

                // 플레이어를 감지할 레이저를 쏘는 부분
                RaycastHit hit;
                GoalPosition = target.position;
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
                        laserLine.enabled = false; // 플레이어 감지 못하면 레이저 끄기
                    }
                }
                else
                {
                    laserLine.enabled = false; // 정해진 각도 외에 플레이어 감지 못함
                }
            }
            else
            {
                // 플레이어가 죽으면 레이저 끄기
                laserLine.enabled = false;

                // 죽은 상태에서 타이머가 다 끝나면 터렛이 죽은 상태로 처리
                dyingTimer -= Time.deltaTime;
                if (dyingTimer <= 0f)
                {
                    RotateAndFire(); // 죽기 직전까지 공격 
                }
                else
                {
                    StopTurret(); // 터렛 중지
                }
            }
        }
        else
        {
            // 플레이어 오브젝트가 없다면 레이저 끄기
            laserLine.enabled = false;
        }
    }

    void DrawLaser(Vector3 targetPosition)
    {
        if (laserLine != null)
        {
            laserLine.enabled = true; // 레이저 활성화

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

    void RotateTowardsPlayer(Transform target)
    {
        if (target != null)
        {
            // 플레이어의 위치로 회전 방향 계산
            Vector3 direction = target.position - transform.position;
            direction.y = 0;  // Y축을 고정하여 위아래 회전 방지

            // 목표 회전 방향 계산 (Y축만 회전)
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 터렛 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FireBullet() // 탄환 발사
    {
        // 발사된 탄환을 생성
        ObjectPool.SpawnFromPool("Bullet", firePoint.position, firePoint.rotation); // ObjectPool에서 Bullet을 Spawn하도록
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);  // 힘 조절
                rb.AddTorque(Vector3.right * 1f, ForceMode.Impulse); // 회전력 조절
            }

            isDying = true; // 터렛 중지
        }
    }

    public void StopTurret()
    {
        isDying = true;
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
