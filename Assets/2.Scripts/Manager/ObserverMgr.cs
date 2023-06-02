using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

public class ObserverMgr : MgrBase
{
    public static ObserverMgr instance = null;

    /// <summary> ��Ƽ ��� ��ųʸ� </summary>
    private static Dictionary<eObserverNoticeType, List<Action>> noticeDic;

    public override void Init()
    {
        //1.�ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //��Ƽ ���
        noticeDic = new Dictionary<eObserverNoticeType, List<Action>>();
    }

    public override void Refresh()
    {
        base.Refresh();
    }

    #region ��Ƽ ���,����,ȣ��
    /// <summary>��Ƽ ���</summary>
    /// <returns>��� �����ϸ� true ��ȯ</returns>
    public static bool Register(eObserverNoticeType observerNoticeType, Action action)
    {
        //�ش� Ÿ�� ����� �ִ��� Ȯ��
        if(noticeDic.ContainsKey(observerNoticeType))
        {
            //Ÿ�� ��� �ȿ� �ش� �׼��� �ִ��� Ȯ��
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
        //�ش� ����� ���ٸ�
        else
        {
            List<Action> actions = new List<Action>();
            actions.Add(action);
            noticeDic.Add(observerNoticeType, actions);

            return true;
        }
    }

    /// <summary>��Ƽ ����</summary>
    /// <returns>���� �����ϸ� true ��ȯ</returns>
    public static bool DeRegister(eObserverNoticeType observerNoticeType, Action action)
    {
        //�ش� Ÿ�� ����� �ִ��� Ȯ��
        if (noticeDic.ContainsKey(observerNoticeType))
        {
            //Ÿ�� ��� �ȿ� �ش� �׼��� �ִ��� Ȯ��
            if (noticeDic[observerNoticeType].Contains(action))
            {
                noticeDic[observerNoticeType].Remove(action);
                return true;
            }
        }

        return false;
    }

    /// <summary>��Ƽ ȣ��</summary>
    public static void NoticeCall(eObserverNoticeType observerNoticeType)
    {
        //�ش� Ÿ�� ����� �ִ��� Ȯ��
        if (noticeDic.ContainsKey(observerNoticeType))
        {
            for(int i = 0; i < noticeDic[observerNoticeType].Count; ++i)
            {
                noticeDic[observerNoticeType][i]();
            }
        }
    }
    #endregion ��Ƽ ���,����,ȣ��

}
