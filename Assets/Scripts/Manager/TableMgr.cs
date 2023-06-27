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

        //���̺� ����
        SetTableDatas();
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
        Type tp = typeof(T);
        T tbl = tp as T;


        if(tbl.GetKey is string)
        {

        }
        else if(tbl.GetKey is int)
        {
             
        }
        else if (tbl.GetKey is long)
        {

        }
        else if ( tbl.GetKey is bool) 
        {

        }
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