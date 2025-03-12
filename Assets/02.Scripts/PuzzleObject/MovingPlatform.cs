using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("MovingPlatform Info")]
    [Tooltip("Platform의 부모가 되는 오브젝트")]
    public Transform parentObj;
    [Tooltip("월드상에 wayPoints 하나 이상 필수 생성")]
    [SerializeField] List<Transform> wayPoints;

    [Tooltip("플랫폼 이동 속도")]
    [Range(1f,50f)]
    [SerializeField] float moveSpeed = 5f;
    [Tooltip("wayPoint 도착 후 대기 시간")]
    public float waitTime = 0.5f; 

    [Tooltip("생성 시 동작 유무")]
    [SerializeField] bool isPlayOnAwake = true;
    [SerializeField] bool isLoop = true;

    private Rigidbody _rigidbody;
    private Transform targetPos; // 목표 도착 위치
    private int wayPointCnt; //웨이 포인트 수
    private int curPointIdx; // 가야하는 포인트 인덱스


    void Awake()
    {
        wayPointCnt = wayPoints.Count;
        if (wayPointCnt == 0)
            Debug.LogError("WayPoints is Null");

        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (targetPos == null)
        {
            targetPos = transform;

            //같은 부모 위치에 Transform 생성
            Transform a = new GameObject().GetComponent<Transform>();
            a.parent = parentObj;
            SwapWayPoint(a);
        }
        if (isPlayOnAwake == true)
            Play();
    }

    public void Play()
    {
        StartCoroutine(nameof(Process));
    }

    private IEnumerator Process()
    {
        var wait = new WaitForSeconds(waitTime);

        while (true)
        {
            // wayPoints[curPointIdx].position 까지 이동
            yield return StartCoroutine(MoveAToB(targetPos.position, wayPoints[curPointIdx].position));

            if(curPointIdx < wayPointCnt - 1)
            {
                curPointIdx++;
            }
            else
            {
                //true시 가야할 idx를 시작지점으로 설정
                if (isLoop == true)
                    curPointIdx = 0;
                else
                    break;
            }
            yield return wait;
        }
    }

    private IEnumerator MoveAToB(Vector3 start, Vector3 end)
    {
        float percent = 0;
        //이동 시간 = 총 이동거리 / 초당 이동 거리
        float moveTime = Vector3.Distance(start, end)/ moveSpeed;

        //start에서 end까지의 위치이동이 끝날때까지 반복
        while(percent < 1)
        {
            percent += Time.deltaTime / moveTime;

            targetPos.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }
    }

    //추후 큐브나 다른 오브젝트도 추가해야한다.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }

    //현재위치를 0번인덱스로 바꿔주는 함수
    private void SwapWayPoint(Transform transform)
    {
        var temp = wayPoints[0];
        wayPoints[0] = transform;
        wayPoints.Insert(1,temp);
        wayPointCnt++;
    }
}
