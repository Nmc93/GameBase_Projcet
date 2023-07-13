using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;
using UnityEngine.UI;
using System;

[Serializable]
public abstract class UIBase : MonoBehaviour
{

    /// <summary> ĵ���� Ÿ�� </summary>
    public abstract eCanvas canvasType { get; }

    /// <summary> �ش� UI�� Ÿ�� </summary>
    public abstract eUI uiType { get; }

    /// <summary> �� ��ȯ�� ����Ǵ��� ���� </summary>
    public bool IsSceneChangeClose { get => isSceneChangeClose; }

    /// <summary> UI ���� ���� </summary>
    public bool IsOpen { get; private set; }

    [Header("---------------- Base ----------------")]
    [Header("[�� ��ȯ�� ����Ǵ��� ����]"),Tooltip("true : �� ����� UI ����\nfalse: �� ����� UI ������")]
    [SerializeField] private bool isSceneChangeClose = true;

    public virtual void Init()
    {

    }

    public virtual void Open()
    {
        IsOpen = true;
        DataSetting();
    }


    public virtual void DataSetting()
    {
        Refresh();
    }

    public virtual void Refresh()
    {

    }

    public virtual void Close()
    {
        DataClear();
    }

    public virtual void DataClear()
    {
        BackThePool();
    }

    public virtual void BackThePool()
    {
        IsOpen = false;
        UIMgr.instance.ReturnToUIPool(this);
    }

}
