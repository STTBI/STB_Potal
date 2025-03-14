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



    void Update()
    {
        if (!isDying)
        {
            // 플레이어를 감지할 레이저를 쏘는 부분
            RaycastHit hit;
            Vector3 direction = target.position - transform.position;

            // 레이저를 플레이어를 향해 쏘기
            if (Physics.Raycast(transform.position, direction.normalized, out hit, detectionRange))
            {
                // 레이저가 플레이어와 충돌하면
                if (hit.collider.transform == target)
                {
                    // 플레이어를 감지한 경우
                    RotateAndFire();
                }
            }
        }
        else
        {
            dyingTimer -= Time.deltaTime;
            if(dyingTimer <= 0f)
            {
                StopTurret();
            }
        }

        // !!테스트용 끝나고 삭제!!
        if (Input.GetKeyDown(KeyCode.T))
        {
            Die();
        }
    }

    void RotateAndFire()
    {
        if (isDying) return;

        // 타겟을 향해 회전
        Vector3 direction = target.position - transform.position;
        direction.y = 0;  // y축 회전 안함 (수평 회전만 하게 함)

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 발사 타이밍 확인
        if (Time.time > nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
   
    void FireBullet()// 탄환 발사
    {
        // 발사된 탄환을 생성
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    public void Die()
    {
        isDying = true;
        dyingTimer = 3f;
    }

    public void StopTurret()
    {
        // !!테스트용 끝나고 삭제!!
        GetComponent<Renderer>().material.color = Color.red; // 터렛 작동 중지시 빨간색

        isDying = false;
        nextFireTime = Mathf.Infinity;
        target = null;
    }
}
