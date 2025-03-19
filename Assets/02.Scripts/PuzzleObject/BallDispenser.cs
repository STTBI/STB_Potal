using UnityEngine;

public class BallDispenser : MonoBehaviour
{
    private Ball _activeBall; //Projectile prefab

    [Tooltip("Ball Regen Cool Down")]
    public float RegenTime;

    private void Awake()
    {
        _activeBall = Resources.Load<Ball>("Prefabs/PuzzleObject/Ball");
    }

    private void Start()
    {
        CreateBall();
    }

    void CreateBall()
    {
        //Create ActiveBall Prefab
        Ball activeBall = Instantiate<Ball>(_activeBall, transform.position + (transform.up * 0.5f), transform.rotation);
        //Add Method to event
        activeBall.BallDestroy += DelayCreateTime;
    }

    void DelayCreateTime() //Create Delay
    {
        Invoke(nameof(CreateBall),RegenTime);
    }

}
