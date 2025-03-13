using System;
using UnityEngine;

public class BallDispenser : MonoBehaviour
{
    [Tooltip("발사체")]
    public Ball ActiveBall;
    [Tooltip("공 생성 쿨타임")]
    public float RegenTime;

    private void Start()
    {
        CreateBall();
    }

    void CreateBall()
    {
        //ActiveBall 프리팹 생성
        Ball activeBall = Instantiate<Ball>(ActiveBall, transform.position + (transform.up * 0.5f), transform.rotation);
        //파괴될때 호출할 메서드 등록
        activeBall.BallDestroy += DelayCreateTime;
    }

    void DelayCreateTime() //생성 딜레이 
    {
        Invoke(nameof(CreateBall),RegenTime);
    }

}
