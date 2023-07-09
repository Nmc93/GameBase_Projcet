using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMgr : MgrBase
{
    public static OptionMgr instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary> bool 타입의 옵션을 불러옴 </summary>
    /// <param name="key">OptionTableData의 ID</param>
    /// <returns> 받아온 테이블 데이터에 문제가 있을 경우 false 반환 </returns>
    public static bool GetBoolOption(string id)
    {
        string data = TableMgr.Get<OptionTableData>(id).OptionValue;

        if (string.IsNullOrEmpty(data))
        {
            Debug.LogError($"{id}의 ID를 가진 옵션을 찾을 수 없습니다.");
            return false;
        }
        else if (data != "TRUE" && data == "FALSE")
        {
            Debug.LogError($"{id} 옵션의 값{data}은(는) bool 타입이 아닙니다.");
            return false;
        }

        return data == "TRUE" ? true : false;
    }
}
