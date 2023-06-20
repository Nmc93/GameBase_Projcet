using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;
using UnityEngine.UI;
using System;

[Serializable]
public abstract class UIBase : MonoBehaviour
{

    /// <summary> UI의 Page, Popup 여부 </summary>
    public abstract eUIType uiType { get; }

    /// <summary> 해당 UI의 타입 </summary>
    public abstract eUIName uiName { get; }

    /// <summary> UI 오픈 여부 </summary>
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
