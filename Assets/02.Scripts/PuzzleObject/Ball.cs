using System;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //Create Ball Event
    public event Action BallDestroy;

    private Vector3 _direction; // move Dir
    private Rigidbody _rigidbody;
    private bool _isCatched = false;

    [Tooltip("Ball Speed")]
    public float ballSpeed = 5f;
    [Tooltip("Reflection Count")]
    public int reflectCount = 4;
    [Tooltip("Effect on Destroy")]
    public bool fxOnDestroy = true;
    private Collider cd;

    private void Awake()
    {
        //GetComponent 
        if (!TryGetComponent<Rigidbody>(out _rigidbody))
            Debug.LogError("Ball rigidbody is Null");
    }
    private void Start()
    {
        //Set dir,speed
        _direction = transform.up;
        _rigidbody.velocity = _direction * ballSpeed;
        cd = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Portal"))
        {
            cd.isTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Portal"))
        {
            cd.isTrigger = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_isCatched) return;

        //is can Reflect 
        if (!cd.isTrigger && reflectCount > 0)
        {
            //_direction : MoveDir , GetContact : Vertical Vector dir
            _direction = Vector3.Reflect(_direction, collision.GetContact(0).normal);
            transform.up = _direction;
            //Set Speed,Dir
            _rigidbody.velocity = _direction * ballSpeed;
            reflectCount--;
        }
        else //Reflect Count  <= 0 
            DestroyBall();
    }
    public void OnCatched()
    {
        // make velocity zero , change color
        _isCatched = true;
        _rigidbody.velocity = Vector3.zero;
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    void DestroyBall()
    {
        //Process Event Method
        BallDestroy?.Invoke();

        Destroy(gameObject);
    }
}