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

    /// <summary> bool Ÿ���� �ɼ��� �ҷ��� </summary>
    /// <param name="key">OptionTableData�� ID</param>
    /// <returns> �޾ƿ� ���̺� �����Ϳ� ������ ���� ��� false ��ȯ </returns>
    public static bool GetBoolOption(string id)
    {
        string data = TableMgr.Get<OptionTableData>(id).OptionValue;

        if (string.IsNullOrEmpty(data))
        {
            Debug.LogError($"{id}�� ID�� ���� �ɼ��� ã�� �� �����ϴ�.");
            return false;
        }
        else if (data != "TRUE" && data == "FALSE")
        {
            Debug.LogError($"{id} �ɼ��� ��{data}��(��) bool Ÿ���� �ƴմϴ�.");
            return false;
        }

        return data == "TRUE" ? true : false;
    }
}
