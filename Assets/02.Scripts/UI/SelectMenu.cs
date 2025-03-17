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
        //�ٷ� ���� �� �̵�?
        //�ƴϸ� �ܰ� ������ �� �ֵ���?
    }

    public void OnLoadGameButton()
    {
        //���̺� ���� ��� �ҷ�����

        //�ٵ� ���� ��Ʈ�ѷ��� �־�� �� �� ����
        //����� ��������� Empty �� ����شٴ���...
        //
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
