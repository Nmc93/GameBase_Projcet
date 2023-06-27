using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMgr : MgrBase
{
    public static TableMgr instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary> ���̺� ���� </summary>
    private void SetTableDatas()
    {
        //���̺� �����͸� ����
        LoadTable<StringTableData>();
    }

    /// <summary> ������ ���̺��� �ε� </summary>
    /// <typeparam name="T"> ���̺� Ŭ������ ���� </typeparam>
    private void LoadTable<T>() where T : TableBase
    {
        List<T> tableList = new List<T>();
        Type tbl = typeof(T);
        T ff = tbl as T;
    }
}

/// <summary> ���̺� ������ </summary>
public class TableData<T> where T : TableBase
{
    public T this[string key]
    {
        get
        {
            return null;
        }
    }

    /// <summary>  </summary>
    public List<T> list = new List<T>();
}