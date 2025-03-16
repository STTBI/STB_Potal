using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    public Button NewGame;
    public Button LoadGame;
    public Button Option;
    public Button Achievements;
    public Button Quit;

    private void OnValidate()
    {
        NewGame = transform.Find("NewGame").GetComponent<Button>();
        LoadGame = transform.GetChild(1).GetComponent<Button>();
        Option = Utill.FindChildRecursive(transform, "Option").GetComponent<Button>();
        Achievements = Utill.FindChildRecursive(transform, "Achievements").GetComponent<Button>();
        Quit = Utill.FindChildRecursive(transform, "Quit").GetComponent<Button>();
    }

    public void OnNewGameButton()
    {
        //바로 게임 씬 이동?
        //아니면 단계 선택할 수 있도록?
    }

    public void OnLoadGameButton()
    {
        //세이브 파일 목록 불러오기

        //근데 슬롯 컨트롤러도 있어야 할 것 같음
        //목록이 비어있으면 Empty 를 띄워준다던가...
        //
    }

    public void OnOptionButton()
    {
        //옵션에서 뭘 설정할건지 정해야함
        //사운드 넣는다면 사운드 조절
        //창모드 전체모드?
        //게임 켜보고 뭐 설정할 수 있는지 한 번 확인해보기
    }

    public void OnAchievementsButton()
    {
        //업적
        //원작에 있고 추가과제에도 있어서 일단 만들어놓음
        //일단 맵 클리어 할 때마다 하나씩 줘도 괜찮을 것 같음
        //이건 컨텐츠가 좀 나오면 상의해서 넣는 편이 좋아보임
    }

    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

}
