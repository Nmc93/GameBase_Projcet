using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

// 1. Awake�ܿ��� �Ŵ����� �����ϰ� �����
// 2. ������ ������� �Ŵ����� �ʱ�ȭ ��Ŵ(������ ���� ���� ����)

public class GameManager : MonoBehaviour
{
    /// <summary> �Ŵ��� Ŭ���� ���� </summary>
    public static Dictionary<eMgr, MgrBase> mgrDic;

    private void Awake()
    {
        //�� �Ŵ���
        GameObject sceneMgrObj = new GameObject();
        mgrDic.Add(eMgr.SceneMgr, sceneMgrObj.AddComponent<SceneMgr>());

        //UI �Ŵ���
        GameObject uiMgrObj = new GameObject();
        mgrDic.Add(eMgr.UIMgr, uiMgrObj.AddComponent<UIMgr>());
    }

    #region Get

    /// <summary> �Ŵ��� Ŭ������ ��ȯ�ϴ� �Լ� </summary>
    /// <param name="type"> �Ŵ����� enum Ÿ�� </param>
    public static MgrBase GetMgr(eMgr type)
    {
        if (!mgrDic.TryGetValue(type, out MgrBase mgr))
        {
            Debug.LogError($"{type}�� �ش��ϴ� �Ŵ����� �����ϴ�.");
            return null;
        }

        return mgr;
    }

    #endregion Get
}