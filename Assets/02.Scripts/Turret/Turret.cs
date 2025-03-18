using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;  // 플레이어
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
    }


    void Update()
    {
        // 터렛을 드는 기능
        if (Input.GetKeyDown(KeyCode.C))
        {
            LiftTurretByPlayer();
        }

        //  터렛을 놓는 기능
        if (Input.GetKeyDown(KeyCode.X))
        {
            DropTurret();
        }

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
                Vector3 direction = target.position - transform.position;

                // 정면에서만 감지하게 각도 조절
                float angle = Vector3.Angle(transform.forward, direction);
                if (angle <= maxRotationAngle)
                {
                    // 레이저를 플레이어를 향해 쏘기
                    if (Physics.Raycast(transform.position, direction.normalized, out hit, detectionRange))
                    {
                        // 레이저가 플레이어와 충돌하면
                        if (hit.collider.transform == target)
                        {
                            // 플레이어를 감지한 경우
                            RotateAndFire();
                            DrawLaser(hit.point); // 레이저 그리기
                        }
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

    void DrawLaser(Vector3 tartgetposition)
    {
        if(laserLine != null)
        {
            laserLine.enabled = true; //레이저 활성화

            laserLine.positionCount = 2;

            laserLine.SetPosition(0, firePoint.position); // 시작점
            laserLine.SetPosition(1, tartgetposition); // 타겟의 위치
        }
    }

    void RotateAndFire()
    {
        if (isDying) return;

        // 타겟을 향해 회전
        Vector3 direction = target.position - transform.position;
        direction.y = 0;  // y축 회전 안함

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 발사 타이밍 확인
        if (Time.time > nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
   
    void FireBullet() // 탄환 발사
    {
        // 발사된 탄환을 생성
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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

    /*
     코드 수정하실꺼
    
    FireBullet함수 아래로 변경 ObjectPool에서 Bullet을 Spawn하도록
    void FireBullet() // 탄환 발사
    {
        // 발사된 탄환을 생성
        ObjectPool.SpawnFromPool("Bullet", firePoint.position, firePoint.rotation);
    }

     */

}
