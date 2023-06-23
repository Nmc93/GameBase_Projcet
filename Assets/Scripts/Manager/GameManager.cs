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
    public static Dictionary<eMgr, MgrBase> mgrDic = new Dictionary<eMgr, MgrBase>();

    /// <summary> ���۽� ���� �Ŵ��� ���� </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //���̺� �Ŵ��� ����
        GameObject tableMgr = new GameObject();
        tableMgr.name = "TableMgr";
        mgrDic.Add(eMgr.TableMgr, tableMgr.AddComponent<TableMgr>());

        //UI �Ŵ��� ����
        GameObject uiMgrObj = new GameObject();
        uiMgrObj.name = "UIMgr";
        mgrDic.Add(eMgr.UIMgr, uiMgrObj.AddComponent<UIMgr>());

        //�� �Ŵ���
        GameObject sceneMgrObj = new GameObject();
        sceneMgrObj.name = "SceneMgr";
        mgrDic.Add(eMgr.SceneMgr, sceneMgrObj.AddComponent<SceneMgr>());

        #region ���� �߰�

        ////���̺� �Ŵ���
        //GameObject saveMgrObj = new GameObject();
        //saveMgrObj.name = "SaveMgr";
        //mgrDic.Add(eMgr.SaveMgr, saveMgrObj.AddComponent<SaveMgr>());

        #endregion ���� �߰�
    }

    /// <summary> �Ŵ��� ���� �� ���� ���� </summary>
    private void Start()
    {
        
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