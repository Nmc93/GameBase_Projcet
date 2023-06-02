using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

public abstract class UIBase : MonoBehaviour
{
    /// <summary> 해당 UI의 종류 </summary>
    protected abstract eUIPrefab PrefabType { get; }

    private void OnEnable()
    {
        Init();
    }

    //UI 활성화될때 호출됨
    public abstract void Init();

    /// <summary> UI 종료 버튼에 사용될 함수 </summary>
    public virtual void OnClickClose()
    {
        Close();
    }

    /// <summary> UI 종료 함수 </summary>
    public void Close()
    {
        UIMgr.instance.CloseUI(PrefabType);
    }
}
