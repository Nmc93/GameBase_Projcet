using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

public class UIMgr : MgrBase
{
    public static UIMgr instance = null;

    /// <summary>UI�� �����ϴ� ��ųʸ�</summary>
    private Dictionary<eUIPrefab, GameObject> uiBaseDic = new Dictionary<eUIPrefab, GameObject>();

    private Dictionary<eUIPrefab, UIPrefabData> uiDic = new Dictionary<eUIPrefab, UIPrefabData>();

    [SerializeField]
    private List<eUIPrefab> activePrefabList = new List<eUIPrefab>();

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

        //UI ����� üũ
        if (uiDic == null)
        {
            uiDic = new Dictionary<eUIPrefab, UIPrefabData>();
        }

        #region Scene UI
        #endregion Scene UI

        #region Page UI
        #endregion Page UI

        #region Popup UI

        //�ɼ� �˾�
        uiDic.Add(eUIPrefab.OptionPopup, new UIPrefabData(eUIType.Popup, "Prefab/UI/OptionPopup"));

        #endregion Popup UI
    }

    #region ������ Ȱ��ȭ ��Ȱ��ȭ

    /// <summary> UI�� Ȱ��ȭ��, Ǯ�� �ִٸ� Ǯ���� �������� �ƴϸ� ������ </summary>
    /// <param name="uIPrefab"> UI�̸� </param>
    /// <param name="canvas"> UI�� ����ؾ��� ĵ���� </param>
    /// <param name="uiPosition"> UI�� ��ġ </param>
    public void OpenUI(eUIPrefab uIPrefab, GameObject canvas, Vector3 uiPosition)
    {
        GameObject uiObjcet = null;

        //Ǯ�� �������� ���� ���
        if (PoolMgr.instance.GetUIPool(uIPrefab, out uiObjcet))
        {
            uiObjcet.transform.SetParent(canvas.transform);
            uiObjcet.transform.position = uiPosition;
            uiObjcet.transform.localScale = new Vector3(1, 1, 1);

            //�׽�Ʈ
            activePrefabList.Add(uIPrefab);
            uiBaseDic.Add(uIPrefab, uiObjcet);

            //�Ŵ��� ������ �������� Ȱ��ȭ
            uiBaseDic[uIPrefab].SetActive(true);
        }
        //Ǯ�� �������� ���� ���
        else
        {
            //������ ����
            uiObjcet = CreateUI(uIPrefab);
            uiObjcet.transform.SetParent(canvas.transform);
            uiObjcet.transform.position = uiPosition;
            uiObjcet.transform.localScale = new Vector3(1, 1, 1);

            //�׽�Ʈ
            activePrefabList.Add(uIPrefab);
            uiBaseDic.Add(uIPrefab, uiObjcet);

            //�Ŵ��� ������ �������� Ȱ��ȭ
            uiBaseDic[uIPrefab].SetActive(true);
        }
    }

    /// <summary> �Ŵ������� �������� �������� Ǯ�� �ְ� ������ </summary>
    public void CloseUI(eUIPrefab uIPrefab)
    {
        GameObject uiObjcet = null;

        //�������� ���� Ȱ��ȭ �������� Ȯ��
        if(uiBaseDic.TryGetValue(uIPrefab,out uiObjcet))
        {
            //UI�Ŵ������� �������̴� �������� ����
            uiBaseDic.Remove(uIPrefab);

            //�׽�Ʈ��
            for(int i = 0; i < activePrefabList.Count; ++i)
            {
                if(activePrefabList[i] == uIPrefab)
                {
                    activePrefabList.RemoveAt(i);
                    break;
                }
            }

            //Ǯ�� �������� ����
            PoolMgr.instance.SetUIPool(uIPrefab, uiObjcet);
        }
    }
    #endregion ������ Ȱ��ȭ ��Ȱ��ȭ

    #region ������ ����
    /// <summary> ������ ���� </summary>
    /// <returns> �������� ���ٸ� Null ��ȯ </returns>
    public GameObject CreateUI(eUIPrefab uIPrefab)
    {
        GameObject uiObjcet = null;

        if (uiDic.TryGetValue(uIPrefab,out UIPrefabData data))
        {
            uiObjcet = Instantiate(Resources.Load(data.path)) as GameObject;
        }
        else
        {
            Debug.LogError($"{uIPrefab}�� �ش��ϴ� �������� ã�� �� �����ϴ�.");
        }

        return uiObjcet;
    }

    #endregion ������ ����
}

[Serializable]
public class UIPrefabData
{
    public UIPrefabData(eUIType uIType, string path, UIBase uiClass = null)
    {
        this.uIType = uIType;
        this.path = path;
        this.uiClass = uiClass;
    }

    /// <summary> UI ���� </summary>
    public eUIType uIType = eUIType.None;
    /// <summary> ������ ��� </summary>
    public string path = null;
    /// <summary> UI�� ���� Ŭ���� </summary>
    public UIBase uiClass = null;
}
