using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed = 5f; // 탄환 속도
    public float lifespawn = 5f; // 탄환 수명

    void Start()
    {
        Destroy(gameObject, lifespawn); //탄환 발사 후 일정 시간 지난 뒤 파괴
    }

    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime); //탄환의 전방으로 계속 이동
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(gameObject); // 플레이어와 충돌시 탄환 삭제
        }
    }

    /*
    Start함수를 OnEnable로 변경

    private void OnEnable()
    {
        Invoke(nameof(UnActive), lifespawn);
    }

    Invoke로 lifespawn후에 호출될 함수 추가 자신을 비활성화
    void UnActive() => gameObject.SetActive(false); 

    비활성화 될시 오브젝트풀에서 대기 Invoke중지시켜주기 위한 CancelInvoke
    void OnDisable()
    {
        ObjectPool.ReturnToPool(gameObject);
        CancelInvoke();
    }

    스크립트에서 Destroy(gameObject) 전부 UnActive() 로 변경하면됩니다.

    씬에서 테스트 하실때 추가할거
    CreateEmpty - ObjectPool 스크립트 붙히기 - Pools에서 + 버튼 - Prefab에 Bullet 프리팹 등록
    Tag에는 프리팹과 동일한 이름, Size에는 오브젝트 풀 크기 ex)20

     */

}
