using System;
using UnityEngine;

public interface ITriggerObj //���� ������� ��ġ
{
    public event Action OnTriggerUpdate;
    bool TriggerCheck { get; set; }
    void TriggerTrue();
    void TriggerFalse();
}

public interface ICheckObj // ���� ���� ���������ġ�� ��� ������ True
{
    bool isTrue { get; set; }
    void True();
    void False();
}

//PuzzleSystem ���� ��� Trigger�� �˻� 
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

        //�̺�Ʈ �Լ� �߰�, ITriggerObj bool������ set�ɶ� Invoke
        foreach (var obj in triggerObject)
        {
            obj.OnTriggerUpdate += Decision;
        }
    }

    public void Decision()
    {
        //��� ITriggerObj�� True�϶��� AllTriggerTrue�� ������
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

    //���ICheckObj�� True �Ǵ� False ȣ��
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
