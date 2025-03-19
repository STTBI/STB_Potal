using UnityEngine;

public class BallTrace : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(UnActive), 2f);
    }

    void UnActive() => gameObject.SetActive(false);


    void OnDisable()
    {
        ObjectPool.ReturnToPool(gameObject);
        CancelInvoke();
    }

}
