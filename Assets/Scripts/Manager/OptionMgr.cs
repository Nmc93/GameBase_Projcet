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

    /// <summary> bool Ÿ���� �ɼ��� �ҷ��� </summary>
    /// <param name="key">OptionTableData�� ID</param>
    /// <returns> �޾ƿ� ���̺� �����Ϳ� ������ ���� ��� false ��ȯ </returns>
    public static bool GetBoolOption(string id)
    {
        //���̺� �˻��� �������� ���
        if(TableMgr.Get(id, out OptionTableData tData))
        {
            string data = tData.OptionValue;

            if (string.IsNullOrEmpty(data))
            {
                Debug.LogError($"{id}�� ID�� ���� �ɼ��� ã�� �� �����ϴ�.");
                return false;
            }
            else if (data != "True" && data != "False")
            {
                Debug.LogError($"{id} �ɼ��� ��{data}��(��) bool Ÿ���� �ƴմϴ�.");
                return false;
            }

            return data == "True" ? true : false;

        }
        //�˻� ������ ���
        else
        {
            Debug.LogError($"ID�� �����ϴ� ���̺��� ã�� �� �����ϴ�. [ID : {id}]");
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
