using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GEnum;

public class PoolMgr : MgrBase
{
    public static PoolMgr instance = null;

    /// <summary>UI�� �����ϴ� ��ųʸ�</summary>
    private Dictionary<eUIPrefab , GameObject> uiBasePool = new Dictionary<eUIPrefab, GameObject>();

    [SerializeField]
    private List<eUIPrefab> inUIPoolList = new List<eUIPrefab>();

    public override void Init()
    {
        //1.�ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>������� �ʴ� UI�� Ǯ�� �����ϴ� �Լ�</summary>
    /// <returns>�̹� UI�� Ǯ�� �ִٸ� false�� ��ȯ </returns>
    public bool SetUIPool(eUIPrefab Type , GameObject uiObject)
    {
        //�ش� UI�� ��ųʸ��� ���� ���
        if(!uiBasePool.ContainsKey(Type))
        {
            //UI�� Ǯ�� �߰�
            uiBasePool.Add(Type, uiObject);
            //�׽�Ʈ��
            inUIPoolList.Add(Type);
            //Ǯ�� �������� ��Ȱ��ȭ
            uiBasePool[Type].SetActive(false);

            return true;
        }

        //�̹� Ǯ�� �־ ���� ����
        return false;
    }


    /// <summary> Ǯ�� �ִ� UI�� �������� ���� ���θ� ��ȯ�ϴ� �Լ� </summary>
    /// <returns> Ǯ�� ���� ��� false�� ��ȯ </returns>
    public bool GetUIPool(eUIPrefab Type, out GameObject uiObject)
    {
        //�ش� UI�� ��ųʸ��� ���� ���
        if (uiBasePool.TryGetValue(Type, out uiObject))
        {
            //�� �̻� Ǯ�� ���� �ʿ䰡 �����Ƿ� Ǯ���� ����
            uiBasePool.Remove(Type);

            //�׽�Ʈ��
            for (int i = 0; i < inUIPoolList.Count; ++i)
            {
                if (inUIPoolList[i] == Type)
                {
                    inUIPoolList.RemoveAt(i);
                    break;
                }
            }

            return true;
        }

        //�ش� UI�� Ǯ�� ����
        return false;
    }

    /// <summary> Ǯ�� �ִ� UI�� ��ȯ�ϴ� �Լ� </summary>
    /// <returns> Ǯ�� ���� ��� null�� ��ȯ </returns>
    public GameObject GetUIPool(eUIPrefab Type)
    {
        GameObject uiObject = null;

        //�ش� UI�� ��ųʸ��� ���� ���
        if (uiBasePool.TryGetValue(Type, out uiObject))
        {
            //�� �̻� Ǯ�� ���� �ʿ䰡 �����Ƿ� Ǯ���� ����
            uiBasePool.Remove(Type);

            //�׽�Ʈ��
            for (int i = 0; i < inUIPoolList.Count; ++i)
            {
                if (inUIPoolList[i] == Type)
                {
                    inUIPoolList.RemoveAt(i);
                    break;
                }
            }

            return uiObject;
        }

        return null;
    }
}
