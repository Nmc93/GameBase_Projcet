using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

public class ObserverMgr : MgrBase
{
    public static ObserverMgr instance = null;

    /// <summary> 노티 목록 딕셔너리 </summary>
    private static Dictionary<eObserverNoticeType, List<Action>> noticeDic;

    public override void Init()
    {
        //1.인스턴스 생성
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //노티 목록
        noticeDic = new Dictionary<eObserverNoticeType, List<Action>>();
    }

    public override void Refresh()
    {
        base.Refresh();
    }

    #region 노티 등록,해제,호출
    /// <summary>노티 등록</summary>
    /// <returns>등록 성공하면 true 반환</returns>
    public static bool Register(eObserverNoticeType observerNoticeType, Action action)
    {
        //해당 타입 목록이 있는지 확인
        if(noticeDic.ContainsKey(observerNoticeType))
        {
            //타입 목록 안에 해당 액션이 있는지 확인
            if(noticeDic[observerNoticeType].Contains(action))
            {
                return false;
            }
            else
            {
                noticeDic[observerNoticeType].Add(action);
                return true;
            }
        }
        //해당 목록이 없다면
        else
        {
            List<Action> actions = new List<Action>();
            actions.Add(action);
            noticeDic.Add(observerNoticeType, actions);

            return true;
        }
    }

    /// <summary>노티 해제</summary>
    /// <returns>해제 성공하면 true 반환</returns>
    public static bool DeRegister(eObserverNoticeType observerNoticeType, Action action)
    {
        //해당 타입 목록이 있는지 확인
        if (noticeDic.ContainsKey(observerNoticeType))
        {
            //타입 목록 안에 해당 액션이 있는지 확인
            if (noticeDic[observerNoticeType].Contains(action))
            {
                noticeDic[observerNoticeType].Remove(action);
                return true;
            }
        }

        return false;
    }

    /// <summary>노티 호출</summary>
    public static void NoticeCall(eObserverNoticeType observerNoticeType)
    {
        //해당 타입 목록이 있는지 확인
        if (noticeDic.ContainsKey(observerNoticeType))
        {
            for(int i = 0; i < noticeDic[observerNoticeType].Count; ++i)
            {
                noticeDic[observerNoticeType][i]();
            }
        }
    }
    #endregion 노티 등록,해제,호출

}
