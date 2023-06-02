using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

public abstract class UIBase : MonoBehaviour
{
    /// <summary> �ش� UI�� ���� </summary>
    protected abstract eUIPrefab PrefabType { get; }

    private void OnEnable()
    {
        Init();
    }

    //UI Ȱ��ȭ�ɶ� ȣ���
    public abstract void Init();

    /// <summary> UI ���� ��ư�� ���� �Լ� </summary>
    public virtual void OnClickClose()
    {
        Close();
    }

    /// <summary> UI ���� �Լ� </summary>
    public void Close()
    {
        UIMgr.instance.CloseUI(PrefabType);
    }
}
