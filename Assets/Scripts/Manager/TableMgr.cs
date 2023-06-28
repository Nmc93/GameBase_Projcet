using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Android;

public class TableMgr : MgrBase
{
    public static TableMgr instance;

    private const string csvPath = "";

    private Dictionary<string, TableData> dicTable = new Dictionary<string, TableData>();

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
                //테이블 데이터 타입
                string[] types = reader.ReadLine().Split(",");
                //테이블의 변수값
                object[] tValue = new object[types.Length];
                //테이블 세팅
                string value;
                string[] values;
                while(true)
                {
                    //저장할게 더 없을때까지 진행
                    value = reader.ReadLine();
                    if(value != null)
                    {
                        values = value.ToString().Split(",");
                        //types와 values의 수는 같음;
                        for (int i = 0; i < values.Length; ++i)
                        {
                            //테이블 하나 완성
                            tValue[i] = GetValue(types[i], values[i]);
                        }
                    }
                    else
                    {
                        break;
                    }

                    //TableBase : 1열, 테이블 1개
                    T tableBase = (T)Activator.CreateInstance(tp, tValue);
                    //테이블 저장
                    tableList.Add(tableBase);
                }
            }
        }

        TableData tableData = new TableData();
    }

    /// <summary> 지정된 타입으로 값을 형변환해서 반환 </summary>
    /// <param name="type"> 값의 타입 </param>
    /// <param name="value"> 테이블값 </param>
    /// <returns> 값이 제대로 지정되지 않으면 string으로 변환 </returns>
    public object GetValue(string type,string value)
    {
        switch(type)
        {
            case "int":
                return int.Parse(value);
            case "long":
                return long.Parse(value);
            case "string":
                return value;
            case "bool":
                return bool.Parse(value);
            default:
                return value;
        }
    }
}

/// <summary> 테이블 데이터 </summary>
public class TableData
{
    public TableBase this[object key]
    {
        get
        {
            TableBase table = null;

            switch (type)
            {
                case "int":
                    {
                        if (key is int)
                        {
                            int val = (int)key;
                            table = tableList.Find(item => (int)item.GetKey == val);
                        }
                    } 
                    break;
                case "long":
                    {
                        if (key is long)
                        {
                            long val = (long)key;
                            table = tableList.Find(item => item.GetKey == key);
                        }
                    }
                    break;
                case "string":
                    {
                        if (key is string)
                        {
                            string val = (string)key;
                            table = tableList.Find(item => item.GetKey == key);
                        }
                    }
                    break;
                case "bool":
                    {
                        if (key is bool)
                        {
                            bool val = (bool)key;
                            table = tableList.Find(item => item.GetKey == key);
                        }
                    }
                    break;
            }
            return table;
        }
    }

    /// <summary> 테이블의 키의 타입 </summary>
    private string type = "string";
    /// <summary> 테이블 딕셔너리 </summary>
    public List<TableBase> tableList = new List<TableBase>();
}