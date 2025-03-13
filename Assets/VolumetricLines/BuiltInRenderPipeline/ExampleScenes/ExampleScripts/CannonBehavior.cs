using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonBehavior : MonoBehaviour
{
    public Transform m_muzzle;
    public GameObject m_shotPrefab;
    public float fireRate = 1f; // 발사 간격 (초)
    public int poolSize = 10; // 풀 크기
    private float nextFireTime = 0f;

    private Queue<GameObject> shotPool = new Queue<GameObject>();

    void Start()
    {
        // 오브젝트 풀링: 발사체 미리 생성
        for (int i = 0; i < poolSize; i++)
        {
            GameObject shot = CreateNewShot();
            shotPool.Enqueue(shot);
        }
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate; // 다음 발사 시간 설정
        }
    }

    void Fire()
    {
        GameObject shot = GetShotFromPool();
        shot.transform.position = m_muzzle.position;
        shot.transform.rotation = m_muzzle.rotation;
        shot.SetActive(true);

        // Rigidbody가 있다면 발사 방향으로 힘을 가함
        Rigidbody rb = shot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = m_muzzle.forward * 15.0f; // 속도 조절
        }

        StartCoroutine(DeactivateShot(shot, 3f));
    }

    GameObject GetShotFromPool()
    {
        if (shotPool.Count > 0)
        {
            return shotPool.Dequeue();
        }
        return CreateNewShot();
    }

    GameObject CreateNewShot()
    {
        GameObject shot = Instantiate(m_shotPrefab);
        shot.SetActive(false);
        return shot;
    }

    IEnumerator DeactivateShot(GameObject shot, float delay)
    {
        yield return new WaitForSeconds(delay);
        shot.SetActive(false);

        if (shotPool.Count < poolSize) // 풀 크기 제한
        {
            shotPool.Enqueue(shot);
        }
        else
        {
            Destroy(shot); // 풀 초과 시 제거
        }
    }
}
