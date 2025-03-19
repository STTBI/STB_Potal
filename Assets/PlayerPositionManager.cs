using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    public Vector3 initalizePoint = new Vector3(2f, 2f, -132f);
    public Vector3 playerPosition; // 기본값 설정
    private GameObject player; // Player 객체를 참조할 변수
    private bool isPlayerPositionLoaded = false; // 위치를 이미 불러왔는지 확인하는 변수

    private static PlayerPositionManager instance;

    public static PlayerPositionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerPositionManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PlayerPositionManager");
                    instance = obj.AddComponent<PlayerPositionManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // 씬 전환 시에도 이 객체를 파괴하지 않도록 설정
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
            isPlayerPositionLoaded = true;
            LoadPlayerPosition();
        }
    }

    // 플레이어 위치 저장
    public void SavePlayerPosition()
    {
        // 현재 플레이어 위치를 저장
        playerPosition = player.transform.localPosition;

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
        if (player != null)
        {
            player.transform.position = playerPosition; // 플레이어 위치 업데이트
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

        playerPosition = initalizePoint; // 기본값으로 초기화

        if (player != null)
        {
            player.transform.position = playerPosition; // 기본 위치 적용
        }

        Debug.Log("Player position reset to default.");
    }
}
