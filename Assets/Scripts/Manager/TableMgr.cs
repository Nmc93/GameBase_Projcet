using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMgr : MgrBase
{
    public static TableMgr instance;

    private const string csvPath = "";

    private void Awake()
    {
        instance = this;

        //테이블 세팅
        SetTableDatas();
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
        Type tp = typeof(T);

        using (var file = new FileStream(csvPath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = new StreamReader(file))
            {
                //첫번째 열(각 열의 타입들)
                string[] types = reader.ReadLine().Split(",");

                //데이터 세팅
                string value;
                string[] values;
                while(true)
                {
                    //저장할게 더 없을때까지 진행
                    value = reader.ReadLine();
                    if(value != null)
                    {
                        values = value.ToString().Split(",");
                        //types.Length == values.Length;
                        for (int i = 0; i < values.Length; ++i)
                        {

                        }
                    }
                    else
                    {
                        break;
                    }
                }
                
                Type t = typeof(int);

                object act = Activator.CreateInstance(tp);//,ObjList);
                tableList.Add(act as T);
            }
        }
    }

    /// <summary>  </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Type GetType(string type)
    {
        switch(type)
        {
            case "int" :
                return typeof(int);
            case "long":
                return typeof(long);
            case "string":
                return typeof(string);
            case "bool":
                return typeof(bool);
            default:
                return typeof(string);
        }
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