using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SelectMenu : MonoBehaviour
{
    public Button NewGame;
    public Button LoadGame;
    public Button Option;
    public Button Achievements;
    public Button Quit;

    public GameObject loadPanel;

    public PlayerPositionManager playerPositionManager;

    [SerializeField]
    public String scene;
    void Start()
    {
        playerPositionManager = FindObjectOfType<PlayerPositionManager>();
    }
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
        playerPositionManager.ResetPlayerPosition();
        SceneManager.LoadScene(scene);
    }

    public void OnLoadGameButton()
    {
        //���̺� ���� ��� �ҷ�����

        //�ٵ� ���� ��Ʈ�ѷ��� �־�� �� �� ����
        //����� ��������� Empty �� ����شٴ���...
        SceneManager.LoadScene(scene);
        
    }

    public void OnOptionButton()
    {
        //�ɼǿ��� �� �����Ұ��� ���ؾ���
        //���� �ִ´ٸ� ���� ����
        //â��� ��ü���?
        //���� �Ѻ��� �� ������ �� �ִ��� �� �� Ȯ���غ���
    }

    public void OnAchievementsButton()
    {
        //����
        //���ۿ� �ְ� �߰��������� �־ �ϴ� ��������
        //�ϴ� �� Ŭ���� �� ������ �ϳ��� �൵ ������ �� ����
        //�̰� �������� �� ������ �����ؼ� �ִ� ���� ���ƺ���
    }

    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }

}
