using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //볼 재 생성 이벤트
    public event Action BallDestroy;

    private Vector3 _direction; // 이동방향
    private Rigidbody _rigidbody;

    [Tooltip("공 속도")]
    public float ballSpeed = 5f;
    [Tooltip("반사 횟수")]
    public int reflectCount = 4;
    [Tooltip("사라질 때 이펙트")]
    public bool fxOnDestroy = true;

    private void Awake()
    {
        //컴포넌트 가져오기
        if (!TryGetComponent<Rigidbody>(out _rigidbody))
            Debug.LogError("Ball rigidbody is Null");
    }
    private void Start()
    {
        //방향,속도 설정
        _direction = transform.up;
        _rigidbody.velocity = _direction * ballSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        //반사 횟수가 남았다면 부딪힐때 반사
        if(reflectCount > 0)
        {
            //이동방향과 반사되는 표면의 수직벡터를 이용해 반사 방향 다시 저장
            _direction = Vector3.Reflect(_direction, collision.GetContact(0).normal);
            _rigidbody.velocity = _direction * ballSpeed;
            reflectCount--;
        }
        else //남은 반사횟수 없이 충돌시 Destroy
            DestroyBall();
    }
    void DestroyBall()
    {
        //Destroy전에 BallDispenser에서 등록한 메서드 실행
        BallDestroy?.Invoke();

        Destroy(gameObject);
    }
}
