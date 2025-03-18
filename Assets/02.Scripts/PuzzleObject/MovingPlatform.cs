using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IPuzzleObj
{
    [Header("MovingPlatform Info")]
    [Tooltip("Playeform parent obj")]
    public Transform parentObj;
    [Tooltip("platform wayPoints(more than 1)")]
    [SerializeField] List<Transform> _wayPoints;

    [Tooltip("platform MoveSpeed")]
    [Range(1f,50f)]
    [SerializeField] float _moveSpeed = 5f;
    [Tooltip("Arrive WaitTime")]
    public float waitTime = 0.5f; 

    [SerializeField] bool _isPlayOnAwake = true;
    [SerializeField] bool _isLoop = true;

    private Rigidbody _rigidbody;
    private Transform _targetPos; // Next Target
    private int _wayPointCnt; //Max WayPoint Count
    private int _curPointIdx; // current Target Idx


    void Awake()
    {
        _wayPointCnt = _wayPoints.Count;
        if (_wayPointCnt == 0)
            Debug.LogError("WayPoints is Null");

        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (_targetPos == null)
        {
            _targetPos = transform;

            //Create transform at current Pos
            Transform a = new GameObject().GetComponent<Transform>();
            a.parent = parentObj;
            a.position = parentObj.position;
            SwapWayPoint(a);
        }
        if (_isPlayOnAwake == true)
            Play();
    }

    public void PuzzleTrue()
    {
        Play();
    }

    public void PuzzleFalse()
    {

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
            // Go to wayPoints[curPointIdx].position 
            yield return StartCoroutine(MoveAToB(_targetPos.position, _wayPoints[_curPointIdx].position));

            if(_curPointIdx < _wayPointCnt - 1)
            {
                _curPointIdx++;
            }
            else
            {
                // next Target to first idx
                if (_isLoop == true)
                    _curPointIdx = 0;
                else
                    break;
            }
            yield return wait;
        }
    }

    private IEnumerator MoveAToB(Vector3 start, Vector3 end)
    {
        float percent = 0;
        // moveTime  = max distance / speed per second
        float moveTime = Vector3.Distance(start, end)/ _moveSpeed;

        //repeat before access at B
        while (percent < 1)
        {
            percent += Time.deltaTime / moveTime;

            _targetPos.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Handle"))
        {
            collision.gameObject.transform.SetParent(transform);
       
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Handle"))
        {
            collision.gameObject.transform.parent = null;
     
        }
    }

    //curPos to 0 idx
    private void SwapWayPoint(Transform transform)
    {
        var temp = _wayPoints[0];
        _wayPoints[0] = transform;
        _wayPoints.Insert(1,temp);
        _wayPointCnt++;
    }

}
