using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzleObj // 하위 TriggerCheckSystem가 모두 True일 시 True
{
    void PuzzleTrue();
    void PuzzleFalse();
}

public class PuzzleSystem : MonoBehaviour
{
    public TriggerCheckSystem[] checkObjects; //CheckObjects 자식 Trigger들이 모두 풀릴경우 True
    public IPuzzleObj puzzleObject; // CheckObjects들이 True가 되면 해금될 퍼즐

    [SerializeField] private int TriggerObjectCount = 0;

    private bool _allCheckTrue = false; //모든 CheckObj가 True일시 퍼즐 해제

    private void Start()
    {
        puzzleObject = GetComponentInChildren<IPuzzleObj>();
        checkObjects = GetComponentsInChildren<TriggerCheckSystem>();

        //이벤트 함수 추가, TriggerCheckSystem의 AllTirggerTrue가 set될때 Invoke
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
