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
                //���̺� ������ Ÿ��
                string[] types = reader.ReadLine().Split(",");
                //���̺��� ������
                object[] tValue = new object[types.Length];
                //���̺� ����
                string value;
                string[] values;
                while(true)
                {
                    //�����Ұ� �� ���������� ����
                    value = reader.ReadLine();
                    if(value != null)
                    {
                        values = value.ToString().Split(",");
                        //types�� values�� ���� ����;
                        for (int i = 0; i < values.Length; ++i)
                        {
                            //���̺� �ϳ� �ϼ�
                            tValue[i] = GetValue(types[i], values[i]);
                        }
                    }
                    else
                    {
                        break;
                    }

                    //TableBase : 1��, ���̺� 1��
                    T tableBase = (T)Activator.CreateInstance(tp, tValue);
                    //���̺� ����
                    tableList.Add(tableBase);
                }
            }
        }

        TableData tableData = new TableData();
    }

    /// <summary> ������ Ÿ������ ���� ����ȯ�ؼ� ��ȯ </summary>
    /// <param name="type"> ���� Ÿ�� </param>
    /// <param name="value"> ���̺� </param>
    /// <returns> ���� ����� �������� ������ string���� ��ȯ </returns>
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

/// <summary> ���̺� ������ </summary>
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

    /// <summary> ���̺��� Ű�� Ÿ�� </summary>
    private string type = "string";
    /// <summary> ���̺� ��ųʸ� </summary>
    public List<TableBase> tableList = new List<TableBase>();
}