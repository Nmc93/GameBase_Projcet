using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMgr : MgrBase
{
    public static OptionMgr instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    #region Get

    /// <summary> ID에 대응하는 Value를 저장함 </summary>
    /// <param name="id"></param>
    /// <param name="isSave"> 프랩스 저장 여부 </param>
    /// <param name="value"> 반환 </param>
    private static bool OptionValue(string id, bool isSave, out string value)
    {
        //값 초기화
        value = string.Empty;

        //프랩스 검색
        if (isSave)
        {
            if (PlayerPrefs.HasKey(id))
            {
                value = PlayerPrefs.GetString(id);
                return true;
            }
        }

        //테이블 검색
        if (TableMgr.Get(id, out OptionTableData tData))
        {
            value = tData.OptionValue;

            //프랩스 저장 타입의 옵션일 경우 프랩스에 저장
            if (isSave)
            {
                PlayerPrefs.SetString(id, value);
                PlayerPrefs.Save();
            }
            return true;
        }

        return false;
    }

    /// <summary> ID의 값을 bool 타입으로 가져옴 </summary>
    /// <param name="id">OptionTableData의 ID</param>
    /// <param name="isSave"> true일 경우 프랩스에 저장한거 확인, true가 디폴트</param>
    /// <returns> 받아온 테이블 데이터에 문제가 있을 경우 false 반환 </returns>
    public static bool GetBoolOption(string id, bool isSave = true)
    {
        if(!OptionValue(id,isSave,out string value))
        {
            Debug.LogError($"{id}의 ID를 가진 데이터를 찾을 수 없습니다.");
            return false;
        }

        //bool로 컨버트
        if (!bool.TryParse(value, out bool result))
        {
            Debug.LogError($"{id}는 bool 타입 옵션이 아니거나 값[{value}]이 잘못 입력되어 있습니다.");
        }

        return result;
    }

    /// <summary> ID의 값을 long 타입으로 가져옴 </summary>
    /// <param name="id">OptionTableData의 ID</param>
    /// <param name="isSave"> true일 경우 프랩스에 저장한거 확인, true가 디폴트</param>
    /// <returns> 받아온 테이블 데이터에 문제가 있을 경우 0 반환 </returns>
    public static long GetLongOption(string id, bool isSave = true)
    {
        if (!OptionValue(id, isSave, out string value))
        {
            Debug.LogError($"{id}의 ID를 가진 데이터를 찾을 수 없습니다.");
            return 0;
        }

        //bool로 컨버트
        if (!long.TryParse(value, out long result))
        {
            Debug.LogError($"{id}는 bool 타입 옵션이 아니거나 값[{value}]이 잘못 입력되어 있습니다.");
        }

        return result;
    }

    /// <summary> ID의 값을 int 타입으로 가져옴 </summary>
    /// <param name="id">OptionTableData의 ID</param>
    /// <param name="isSave"> true일 경우 프랩스에 저장한거 확인, true가 디폴트</param>
    /// <returns> 받아온 테이블 데이터에 문제가 있을 경우 0 반환 </returns>
    public static int GetIntOption(string id, bool isSave = true)
    {
        if (!OptionValue(id, isSave, out string value))
        {
            Debug.LogError($"{id}의 ID를 가진 데이터를 찾을 수 없습니다.");
            return 0;
        }

        //bool로 컨버트
        if (!int.TryParse(value, out int result))
        {
            Debug.LogError($"{id}는 bool 타입 옵션이 아니거나 값[{value}]이 잘못 입력되어 있습니다.");
        }

        return result;
    }

    /// <summary> ID의 값을 float 타입으로 가져옴 </summary>
    /// <param name="id">OptionTableData의 ID</param>
    /// <param name="isSave"> true일 경우 프랩스에 저장한거 확인, true가 디폴트</param>
    /// <returns> 받아온 테이블 데이터에 문제가 있을 경우 0 반환 </returns>
    public static float GetfloatOption(string id, bool isSave = true)
    {
        if (!OptionValue(id, isSave, out string value))
        {
            Debug.LogError($"{id}의 ID를 가진 데이터를 찾을 수 없습니다.");
            return 0;
        }

        //bool로 컨버트
        if (!float.TryParse(value, out float result))
        {
            Debug.LogError($"{id}는 bool 타입 옵션이 아니거나 값[{value}]이 잘못 입력되어 있습니다.");
        }

        return result;
    }

    /// <summary> ID의 값을 string 타입으로 가져옴 </summary>
    /// <param name="id">OptionTableData의 ID</param>
    /// <param name="isSave"> true일 경우 프랩스에 저장한거 확인, true가 디폴트</param>
    /// <returns> 받아온 테이블 데이터에 문제가 있을 경우 string.Empty 반환 </returns>
    public static string GetStringOption(string id, bool isSave = true)
    {
        if (!OptionValue(id, isSave, out string value))
        {
            Debug.LogError($"{id}의 ID를 가진 데이터를 찾을 수 없습니다.");
        }

        return value;
    }

    #endregion Get

}