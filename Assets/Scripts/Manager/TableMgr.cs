using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMgr : MgrBase
{
    public static TableMgr instance;

    /// <summary> csv������ ���� ���� </summary>
    private const string csvPath = "Assets\\Resources\\TableCSV\\{0}.csv";

    /// <summary> ���̺� ������ </summary>
    private Dictionary<string, TableData> dicTable = new Dictionary<string, TableData>();

    private void Awake()
    {
        instance = this;

        //���̺� ����
        SetTableDatas();
    }

    #region ���̺� �ε�
    /// <summary> ���̺� ���� </summary>
    private void SetTableDatas()
    {
        //���̺� �����͸� ����
        //StringTableData ����
        dicTable.Add("StringTableData", LoadTable<StringTableData>());
    }

    /// <summary> ������ ���̺��� �ε� </summary>
    /// <typeparam name="T"> ���̺� Ŭ������ ���� </typeparam>
    private TableData LoadTable<T>() where T : TableBase
    {
        Dictionary<object, TableBase> dicTables = new Dictionary<object, TableBase>(); 
        Type tp = typeof(T);
        string path = tp.ToString();
        path = string.Format(csvPath, path.Substring(0, path.Length - 4));

        using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            using (var reader = new StreamReader(file))
            {
                //���̺� ������ Ÿ��
                string[] types = reader.ReadLine().Split(",");
                //���̺��� ������
                object[] tValue = new object[types.Length];
                while(true)
                {
                    //�����Ұ� �� ���������� ����
                    string value = reader.ReadLine();
                    if(value != null)
                    {
                        string[] values = value.ToString().Split(",");
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
                    dicTables.Add(tableBase.GetKey, tableBase);
                }
            }
        }

        return new TableData(dicTables);
    }
    #endregion ���̺� �ε�

    #region public ��ƿ
    /// <summary> ������ ���̺��� ���� ��ȯ </summary>
    /// <typeparam name="T">������ ���̺�</typeparam>
    /// <param name="key"> ���ϴ� ���̺� �������� ID </param>
    /// <returns> ã�� ���ߴٸ� null ��ȯ </returns>
    public static T Get<T>(object key) where T : TableBase
    {
        Type type = typeof(T);
        if (!instance.dicTable.TryGetValue(type.ToString(), out TableData tbleData))
        {
            UnityEngine.Debug.LogError($"{type}�� ���̺� ������ ã�� �� �����ϴ�.");
        }

        TableBase tb = tbleData[key];
        if(tb == null)
        {
            UnityEngine.Debug.LogError($"{type}���̺��� {key}�� ID�� ���� ������ ã�� �� �����ϴ�.");
        }

        return tb as T;
    }

    /// <summary> ������ ���̺��� ���� �����ϰ� ���� ���θ� ��ȯ </summary>
    /// <typeparam name="T">������ ���̺�</typeparam>
    /// <param name="key">���ϴ� ���̺� �������� ID</param>
    /// <param name="table">���̺� �����͸� ������ �����</param>
    /// <returns>ã�� ���ߴٸ� false ��ȯ</returns>
    public static bool Get<T>(object key, out T table) where T : TableBase
    {
        Type type = typeof(T);
        if (!instance.dicTable.TryGetValue(type.ToString(), out TableData tbleData))
        {
            UnityEngine.Debug.LogError($"{type}�� ���̺� ������ ã�� �� �����ϴ�.");
        }

        table = tbleData[key] as T;
        return table != null;
    }
    #endregion public ��ƿ 

    #region private ��ƿ
    /// <summary> ������ Ÿ������ ���� ����ȯ�ؼ� ��ȯ </summary>
    /// <param name="type"> ���� Ÿ�� </param>
    /// <param name="value"> ���̺� </param>
    /// <returns> ���� ����� �������� ������ string���� ��ȯ </returns>
    private object GetValue(string type,string value)
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
    #endregion private ��ƿ
}

#region ���̺� ������
/// <summary> ���̺� ������ </summary>
[Serializable]
public class TableData
{
    public TableData(Dictionary<object, TableBase> dicTable)
    {
        this.dicTable = dicTable;
    }

    public TableBase this[object key]
    {
        get
        {
            if(!dicTable.TryGetValue(key,out TableBase table))
            {
                UnityEngine.Debug.LogError($"{key}�� ���� ���� �����Ͱ� �����ϴ�.");
            }

            return table;
        }
    }

    /// <summary> ���̺� ��ųʸ� </summary>
    public Dictionary<object, TableBase> dicTable = new Dictionary<object, TableBase>();
}
#endregion ���̺� ������