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

        using (var file = new FileStream(csvPath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = new StreamReader(file))
            {
                //ù��° ��(�� ���� Ÿ�Ե�)
                string[] types = reader.ReadLine().Split(",");

                //������ ����
                string value;
                string[] values;
                while(true)
                {
                    //�����Ұ� �� ���������� ����
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