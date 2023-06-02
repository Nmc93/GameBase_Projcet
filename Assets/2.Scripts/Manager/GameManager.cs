using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. Awake�ܿ��� �Ŵ����� �����ϰ� �����
// 2. ������ ������� �Ŵ����� �ʱ�ȭ ��Ŵ(������ ���� ���� ����)
// 

public class GameManager : MonoBehaviour
{
    public enum eMgr
    {
        ObserverManager = 0,
        OptionManager,
        CanvasMgr,
        SceneManager,
        UIManager,
        PoolManager,
    }

    /// <summary> ���� �Ŵ��� �ν��Ͻ� </summary>
    public static GameManager instance = null;

    /// <summary> �Ŵ��� ��� </summary>
    private Dictionary<eMgr, MgrBase> dic = null;

    #region ����Ƽ ������ ����Ŭ

    //���� ����
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        //�ν��Ͻ� ����
        instance = this;
        DontDestroyOnLoad(gameObject);

        //�Ŵ��� ����� ��ųʸ�
        dic = new Dictionary<eMgr, MgrBase>();

        //������� �Ŵ��� �ʱ�ȭ��
        //�������Ŵ���
        GameObject observObject = new GameObject();
        Add(eMgr.ObserverManager, observObject.AddComponent<ObserverMgr>());

        //�ɼ� �Ŵ���
        GameObject optionObject = new GameObject();
        Add(eMgr.OptionManager, optionObject.AddComponent<OptionMgr>());

        //ĵ���� �Ŵ��� ����
        GameObject CanvasManager = new GameObject();
        Add(eMgr.CanvasMgr, CanvasManager.AddComponent<CanvasMgr>());

        //�� �Ŵ��� ����
        GameObject sceneObject = new GameObject();
        Add(eMgr.SceneManager, sceneObject.AddComponent<SceneMgr>());

        //UI�Ŵ��� ����
        GameObject uiManager = new GameObject();
        Add(eMgr.UIManager, uiManager.AddComponent<UIMgr>());

        //pool�Ŵ��� ����
        GameObject poolManager = new GameObject();
        Add(eMgr.PoolManager, poolManager.AddComponent<PoolMgr>());

    }

    #endregion ����Ƽ ������ ����Ŭ

    #region �Ŵ��� ��ųʸ�

    /// <summary>�Ŵ��� �߰�, �ʱ�ȭ �Լ�</summary>
    /// /// <param name="managerName"> [key] �Ŵ��� �̸� </param>
    /// /// <param name="manager">[Value] �Ŵ��� Ŭ����</param>
    public void Add(eMgr managerName, MgrBase manager)
    {
        if(dic.ContainsKey(managerName))
        {
            Debug.LogError(managerName + ": �̹� �ִ� �Ŵ����Դϴ�.");
            return;
        }

        //�̸�����, �θ�����, �Ŵ��� ��Ͽ� ���, �ʱ�ȭ
        manager.gameObject.name = managerName.ToString();
        manager.transform.SetParent(transform);
        dic.Add(managerName, manager);
        dic[managerName].Init();
        Debug.Log($"Add : {managerName}");
    }

    #endregion �Ŵ��� ��ųʸ�
}