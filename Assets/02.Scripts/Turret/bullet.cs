using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed = 5f; // 탄환 속도
    public float lifespawn = 5f; // 탄환 수명

    private void OnEnable()
    {
        Invoke(nameof(UnActive), lifespawn);
    }

    void UnActive() => gameObject.SetActive(false);

    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime); //탄환의 전방으로 계속 이동
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            UnActive(); // 플레이어와 충돌시 탄환 삭제
        }
    }
    void OnDisable()
    {
        ObjectPool.ReturnToPool(gameObject);
        CancelInvoke();
    }


}
