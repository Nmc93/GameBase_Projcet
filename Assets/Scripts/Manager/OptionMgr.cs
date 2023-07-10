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

    /// <summary> bool 타입의 옵션을 불러옴 </summary>
    /// <param name="key">OptionTableData의 ID</param>
    /// <returns> 받아온 테이블 데이터에 문제가 있을 경우 false 반환 </returns>
    public static bool GetBoolOption(string id)
    {
        //테이블 검색이 성공적일 경우
        if(TableMgr.Get(id, out OptionTableData tData))
        {
            string data = tData.OptionValue;

            if (string.IsNullOrEmpty(data))
            {
                Debug.LogError($"{id}의 ID를 가진 옵션을 찾을 수 없습니다.");
                return false;
            }
            else if (data != "True" && data != "False")
            {
                Debug.LogError($"{id} 옵션의 값{data}은(는) bool 타입이 아닙니다.");
                return false;
            }

            return data == "True" ? true : false;

        }
        //검색 실패의 경우
        else
        {
            Debug.LogError($"ID에 대응하는 테이블을 찾을 수 없습니다. [ID : {id}]");
            return false;
        }
    }


    public static long GetLongOption(string id)
    {
        return 0;
    }

    public static int GetIntOption(string id)
    {
        return 0;
    }

    public static string GetStringOption(string id)
    {
        return string.Empty;
    }
}
