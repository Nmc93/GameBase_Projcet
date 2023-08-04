using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class TableMgr : MgrBase
{
    public static TableMgr instance;

    /// <summary> csv파일의 저장 폴더 </summary>
    private const string csvPath = "Assets\\Resources\\TableCSV\\{0}.csv";
    /// <summary> 바이너리 파일의 저장 폴더 </summary>
    private const string binaryPath = "TableBytes\\{0}";

    /// <summary> 테이블 데이터 </summary>
    private Dictionary<string, TableData> dicTable = new Dictionary<string, TableData>();

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        //테이블 세팅
        SetTableDatas();
    }

    #region 테이블 로드
    /// <summary> 테이블 세팅 </summary>
    private void SetTableDatas()
    {
        //Assets\\Resources\\TableBytes\\StringTableData.bytes
        //테이블 데이터를 세팅

        //StringTableData 세팅
        dicTable.Add("StringTableData", LoadTable<StringTableData>());
        
        //OptionTableData 세팅
        dicTable.Add("OptionTableData", LoadTable<OptionTableData>());

        //OptionTableData 세팅
        dicTable.Add("SoundTableData", LoadTable<SoundTableData>());

        //InputKeyTableData 세팅
        dicTable.Add("InputKeyTableData", LoadTable<InputKeyTableData>());
    }

    /// <summary> 지정된 테이블을 로드 </summary>
    /// <typeparam name="T"> 테이블 클래스만 가능 </typeparam>
    private TableData LoadTable<T>() where T : TableBase
    {
        //해당 타입의 테이블 데이터
        TableData tableData = null;
        //경로 세팅
        string path = string.Format(binaryPath, typeof(T).ToString());

        //경로에 있는 데이터 로드
        TextAsset textAsset = Resources.Load<TextAsset>(path);

        if (textAsset != null)
        {
            //직렬화된 클래스를 역직렬화
            using (MemoryStream stream = new MemoryStream(textAsset.bytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                tableData = bf.Deserialize(stream) as TableData;
            }
        }

        //테이블을 찾을 수 없을 경우
        if (tableData == null)
        {
            Debug.LogError(textAsset == null ? $"{typeof(T)}의 직렬화된 데이터를 찾을 수 없습니다." : $"{typeof(T)}의 역직렬화에 실패했습니다.");
        }

        return tableData;
    }

    #endregion 테이블 로드

    #region public 유틸
    /// <summary> 지정된 테이블의 값을 반환 </summary>
    /// <typeparam name="T">선택할 테이블</typeparam>
    /// <param name="key"> 원하는 테이블 데이터의 ID </param>
    /// <returns> 찾지 못했다면 null 반환 </returns>
    public static T Get<T>(object key) where T : TableBase
    {
        Type type = typeof(T);
        if (!instance.dicTable.TryGetValue(type.ToString(), out TableData tbleData))
        {
            Debug.LogError($"{type}의 테이블 정보를 찾을 수 없습니다.");
        }

        TableBase tb = tbleData[key];
        if(tb == null)
        {
            Debug.LogError($"{type}테이블에서 {key}의 ID를 가진 정보를 찾을 수 없습니다.");
        }

        return tb as T;
    }

    /// <summary> 지정된 테이블의 값을 저장하고 성공 여부를 반환 </summary>
    /// <typeparam name="T">선택할 테이블</typeparam>
    /// <param name="key">원하는 테이블 데이터의 ID</param>
    /// <param name="table">테이블 데이터를 저장할 저장소</param>
    /// <returns>찾지 못했다면 false 반환</returns>
    public static bool Get<T>(object key, out T table) where T : TableBase
    {
        Type type = typeof(T);
        if (!instance.dicTable.TryGetValue(type.ToString(), out TableData tbleData))
        {
            Debug.LogError($"{type}의 테이블 정보를 찾을 수 없습니다.");
            table = null;
            return false;
        }

        table = tbleData[key] as T;
        return table != null;
    }
    #endregion public 유틸 

    #region private 유틸
    /// <summary> 지정된 타입으로 값을 형변환해서 반환 </summary>
    /// <param name="type"> 값의 타입 </param>
    /// <param name="value"> 테이블값 </param>
    /// <returns> 값이 제대로 지정되지 않으면 string으로 변환 </returns>
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
    #endregion private 유틸
}

#region 테이블 데이터
/// <summary> 테이블 데이터 </summary>
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
                UnityEngine.Debug.LogError($"{key}의 값을 가진 데이터가 없습니다.");
            }

            return table;
        }
    }

    /// <summary> 테이블 딕셔너리 </summary>
    public Dictionary<object, TableBase> dicTable = new Dictionary<object, TableBase>();
}
#endregion 테이블 데이터