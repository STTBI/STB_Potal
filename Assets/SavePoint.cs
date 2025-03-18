using UnityEngine;
using System.IO;
using System.Collections;

public class SavePoint : MonoBehaviour
{
    public Camera cameraToUse; // 카메라를 직접 할당할 수 있는 public 변수
    public string saveMessage = "Player position saved at save point!";
    
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 세이브 포인트에 들어오면 위치 저장
        if (other.CompareTag("Player"))
        {
            PlayerPositionManager playerPositionManager = other.GetComponent<PlayerPositionManager>();
            if (playerPositionManager != null)
            {
                playerPositionManager.SavePlayerPosition();
                Debug.Log(saveMessage);
            }
        }
    }

}