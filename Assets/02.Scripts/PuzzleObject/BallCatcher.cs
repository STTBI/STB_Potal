using System;
using UnityEngine;

public class BallCatcher : MonoBehaviour, ITriggerObj
{
    private Ball _catchedBall;

    private bool check;

    public event Action OnTriggerUpdate;

    public bool TriggerCheck
    {
        get
        {
            return check;
        }
        set
        {
            check = value;
            OnTriggerUpdate?.Invoke();
            if (check) TriggerTrue();
            else TriggerFalse();
        }
    }
   
 
    void Start()
    {
        TriggerCheck = false;
    }
    public void TriggerFalse()
    {
        Material mr = gameObject.GetComponentInChildren<MeshRenderer>().material;
        mr.color = Color.red;
    }

    public void TriggerTrue()
    {
        Material mr = gameObject.GetComponentInChildren<MeshRenderer>().material;
        mr.color = Color.green;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Ball>(out _catchedBall))
        {
            TriggerCheck = true;
            _catchedBall.OnCatched();
        }
    }
}
