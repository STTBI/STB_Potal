using UnityEngine;

public class Lava : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Dead");
        }
    }
}
