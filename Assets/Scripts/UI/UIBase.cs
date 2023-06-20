using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;
using UnityEngine.UI;
using System;

[Serializable]
public abstract class UIBase : MonoBehaviour
{

    /// <summary> UI�� Page, Popup ���� </summary>
    public abstract eUIType uiType { get; }

    /// <summary> �ش� UI�� Ÿ�� </summary>
    public abstract eUIName uiName { get; }

    /// <summary> UI ���� ���� </summary>
    public bool IsOpen { get; private set; }

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
        BackThePool();
    }

    public virtual void BackThePool()
    {
        IsOpen = false;
        //UIMgr.
    }

}
