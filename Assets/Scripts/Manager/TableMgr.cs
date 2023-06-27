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

    /// <summary> 테이블 세팅 </summary>
    private void SetTableDatas()
    {
        //테이블 데이터를 세팅
        LoadTable<StringTableData>();
    }

    /// <summary> 지정된 테이블을 로드 </summary>
    /// <typeparam name="T"> 테이블 클래스만 가능 </typeparam>
    private void LoadTable<T>() where T : TableBase
    {
        List<T> tableList = new List<T>();
        Type tbl = typeof(T);
        T ff = tbl as T;
    }
}

/// <summary> 테이블 데이터 </summary>
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