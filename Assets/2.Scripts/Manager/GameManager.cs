using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

// 1. Awake�ܿ��� �Ŵ����� �����ϰ� �����
// 2. ������ ������� �Ŵ����� �ʱ�ȭ ��Ŵ(������ ���� ���� ����)

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    /// <summary> �Ŵ��� Ŭ���� ���� </summary>
    public static Dictionary<eMgr, MgrBase> mgrDic;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //UI �Ŵ���
        GameObject uiMgrObj = new GameObject();
        mgrDic.Add(eMgr.UIMgr, uiMgrObj.AddComponent<UIMgr>());

        //���̺� �Ŵ���
        GameObject saveMgrObj = new GameObject();
        mgrDic.Add(eMgr.SaveMgr, saveMgrObj.AddComponent<SaveMgr>());

        //�� �Ŵ���
        GameObject sceneMgrObj = new GameObject();
        mgrDic.Add(eMgr.SceneMgr, sceneMgrObj.AddComponent<SceneMgr>());
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