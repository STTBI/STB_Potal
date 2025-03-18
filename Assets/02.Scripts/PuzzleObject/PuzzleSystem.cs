using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzleObj  // ���� TriggerCheckSystem�� ��� True�� �� True
{
    void PuzzleTrue();
    void PuzzleFalse();
}

public class PuzzleSystem : MonoBehaviour
{
    public TriggerCheckSystem[] checkObjects; //CheckObjects �ڽ� Trigger���� ��� Ǯ����� True
    public IPuzzleObj puzzleObject; // CheckObjects���� True�� �Ǹ� �رݵ� ����

    [SerializeField] private int TriggerObjectCount = 0;

    private bool _allCheckTrue = false; //��� CheckObj�� True�Ͻ� ���� ����

    private void Start()
    {
        puzzleObject = GetComponentInChildren<IPuzzleObj>();
        checkObjects = GetComponentsInChildren<TriggerCheckSystem>();

        //�̺�Ʈ �Լ� �߰�, TriggerCheckSystem�� AllTirggerTrue�� set�ɶ� Invoke
        foreach (var obj in checkObjects)
        {
            obj.OnPuzzleUpdate += Decision;
        }
    }

    public void Decision()
    {
        for (int i = 0; i < checkObjects.Length; i++)
        {
            if (checkObjects[i].AllTirggerTrue == false)
            {
                _allCheckTrue = false;
                break;
            }
            _allCheckTrue = true;
        }

        IsCheckTrue(_allCheckTrue);
    }

    public void IsCheckTrue(bool val)
    {
        if (!val)
        {
            puzzleObject.PuzzleFalse();
            return;
        }
        else
        {
            puzzleObject.PuzzleTrue();
        }
    }
}
