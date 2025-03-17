using System;
using UnityEngine;

public interface ITriggerObj //퍼즐 잠금해제 장치
{
    public event Action OnTriggerUpdate;
    bool TriggerCheck { get; set; }
    void TriggerTrue();
    void TriggerFalse();
}

public interface ICheckObj // 하위 퍼즐 잠금해제장치가 모두 해제시 True
{
    bool isTrue { get; set; }
    void True();
    void False();
}

//PuzzleSystem 하위 모든 Trigger를 검사 
public class TriggerCheckSystem : MonoBehaviour
{

    public event Action OnPuzzleUpdate;

    public ITriggerObj[] triggerObject;
    public ICheckObj[] checkObject;

    [SerializeField] private bool _allTriggerTrue = false;
    public bool AllTirggerTrue{ 
        get 
        { 
            return _allTriggerTrue;
        }
        set
        {
            _allTriggerTrue = value;
            OnPuzzleUpdate?.Invoke();
        }
    }

    private void Start()
    {
        triggerObject = GetComponentsInChildren<ITriggerObj>();
        checkObject = GetComponentsInChildren<ICheckObj>();

        //이벤트 함수 추가, ITriggerObj bool변수가 set될때 Invoke
        foreach (var obj in triggerObject)
        {
            obj.OnTriggerUpdate += Decision;
        }
    }

    public void Decision()
    {
        //모든 ITriggerObj가 True일때만 AllTriggerTrue를 참으로
        for (int i = 0; i < triggerObject.Length; i++)
        {
            if (triggerObject[i].TriggerCheck == false)
            {
                AllTirggerTrue = false;

                break;
            }
            AllTirggerTrue = true;
        }
        IsTriggerTrue(AllTirggerTrue);
    }

    //모든ICheckObj의 True 또는 False 호출
    public void IsTriggerTrue(bool val)
    {
        for (int i = 0; i < checkObject.Length; i++)
        {
            if (!val)
            {
                checkObject[i].False();
            }
            else
            {
                checkObject[i].True();
            }
        }
    }
}
