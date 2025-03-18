using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    public Vector3 playerPosition = new Vector3(-1, 5.5f, -83); // 기본값 설정
    private GameObject player; // Player 객체를 참조할 변수
    private bool isPlayerPositionLoaded = false; // 위치를 이미 불러왔는지 확인하는 변수

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Player 객체가 씬에 존재하지 않으면 계속 찾음
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        // Player 객체를 찾았으면 위치를 불러와서 설정
        if (player != null && !isPlayerPositionLoaded)
        {
            LoadPlayerPosition();
            isPlayerPositionLoaded = true; // 위치를 불러왔으므로 한 번만 실행
        }
    }

    // 플레이어 위치 저장
    public void SavePlayerPosition()
    {
        PlayerPrefs.SetFloat("PlayerPosX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPosZ", playerPosition.z);
        PlayerPrefs.Save(); // 저장 완료
        Debug.Log("Player position saved.");
    }

   // 저장된 플레이어 위치 불러오기
void LoadPlayerPosition()
{
    if (PlayerPrefs.HasKey("PlayerPosX"))
    {
        float x = PlayerPrefs.GetFloat("PlayerPosX");
        float y = PlayerPrefs.GetFloat("PlayerPosY");
        float z = PlayerPrefs.GetFloat("PlayerPosZ");

        playerPosition = new Vector3(x, y, z); // playerPosition 갱신
    }
    else
    {
        playerPosition = new Vector3(-1, 5.5f, -83); // 기본값
    }

    if (player != null)
    {
        player.transform.position = playerPosition;
        Debug.Log($"Player position loaded: {playerPosition}");
    }
}

// 플레이어 위치를 초기값으로 리셋
public void ResetPlayerPosition()
{
    PlayerPrefs.DeleteKey("PlayerPosX");
    PlayerPrefs.DeleteKey("PlayerPosY");
    PlayerPrefs.DeleteKey("PlayerPosZ");
    PlayerPrefs.Save(); // 변경 사항 저장

    playerPosition = new Vector3(-1, 5.5f, -83); // 기본값으로 초기화

    if (player == null)
    {
        player = GameObject.Find("Player"); // 플레이어 찾기
    }

    if (player != null)
    {
        player.transform.position = playerPosition; // 기본 위치 적용
    }

    Debug.Log("Player position reset to default.");
}
}