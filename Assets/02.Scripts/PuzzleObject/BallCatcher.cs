using UnityEngine;

public class BallCatcher : MonoBehaviour
{
    private Ball _catchedBall;

    private bool isBallCatched = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Ball>(out _catchedBall))
        {
            _catchedBall.OnCatched();
            isBallCatched = true;
        }
    }
}
