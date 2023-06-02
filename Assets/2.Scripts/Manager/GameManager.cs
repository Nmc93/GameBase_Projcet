using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

// 1. Awake단에서 매니저를 생성하고 등록함
// 2. 지정된 순서대로 매니저를 초기화 시킴(순서에 의한 문제 방지)

public class GameManager : MonoBehaviour
{
    /// <summary> 매니저 클래스 저장 </summary>
    public static Dictionary<eMgr, MgrBase> mgrDic;

    private void Awake()
    {
        //씬 매니저
        GameObject sceneMgrObj = new GameObject();
        mgrDic.Add(eMgr.SceneMgr, sceneMgrObj.AddComponent<SceneMgr>());

        //UI 매니저
        GameObject uiMgrObj = new GameObject();
        mgrDic.Add(eMgr.UIMgr, uiMgrObj.AddComponent<UIMgr>());
    }

    #region Get

    /// <summary> 매니저 클래스를 반환하는 함수 </summary>
    /// <param name="type"> 매니저의 enum 타입 </param>
    public static MgrBase GetMgr(eMgr type)
    {
        if (!mgrDic.TryGetValue(type, out MgrBase mgr))
        {
            Debug.LogError($"{type}에 해당하는 매니저가 없습니다.");
            return null;
        }

        return mgr;
    }

    #endregion Get
}