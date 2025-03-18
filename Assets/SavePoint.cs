using UnityEngine;

public class SavePoint : MonoBehaviour
{
    // 세이브 메시지
    public string saveMessage = "Player position saved at save point!";
    
    // 위치가 이미 저장되었는지 확인하는 변수
    private bool isSaved = false;

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 세이브 포인트에 들어오면 위치 저장
        if (other.CompareTag("Player") && !isSaved) // 이미 저장되지 않은 경우에만 실행
        {
            PlayerPositionManager playerPositionManager = PlayerPositionManager.Instance;
            if (playerPositionManager != null)
            {
                playerPositionManager.SavePlayerPosition();  // 위치 저장
                isSaved = true;  // 저장된 상태로 변경
                Debug.Log(saveMessage);  // 저장된 메시지 출력
            }
        }
    }
}
