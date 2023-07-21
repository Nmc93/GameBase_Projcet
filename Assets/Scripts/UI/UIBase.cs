using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;
using UnityEngine.UI;
using System;

[Serializable]
public abstract class UIBase : MonoBehaviour
{

    /// <summary> 캔버스 타입 </summary>
    public abstract eCanvas canvasType { get; }

    /// <summary> 해당 UI의 타입 </summary>
    public abstract eUI uiType { get; }

    /// <summary> 씬 전환시 종료되는지 여부 </summary>
    public bool IsSceneChangeClose { get => isSceneChangeClose; }

    /// <summary> UI 오픈 여부 </summary>
    public bool IsOpen { get; private set; }

    [Header("---------------- Base ----------------")]
    [Header("[씬 전환시 종료되는지 여부]"),Tooltip("true : 씬 종료시 UI 종료\nfalse: 씬 종료시 UI 미종료")]
    [SerializeField] private bool isSceneChangeClose = true;

    public virtual void Init()
    {

    }

    public virtual void Open()
    {
        IsOpen = true;
        DataSetting();
    }

    /// <summary> 데이터 세팅 함수 </summary>
    public virtual void DataSetting()
    {
        Refresh();
    }

    /// <summary> 변경된 데이터에 맞게 UI를 갱신하는 함수 </summary>
    public virtual void Refresh()
    {

    }

    /// <summary> UIMgr에 해당 UI의 종료를 요청하는 함수 </summary>
    public void Close()
    {
        UIMgr.instance.CloseUI(uiType);
    }

    /// <summary> 실제로 UI 종료를 시작하는 함수 </summary>
    public virtual void DataClear()
    {
        BackThePool();
    }

    protected virtual void BackThePool()
    {
        IsOpen = false;
        UIMgr.instance.ReturnToUIPool(this);
    }

}
