using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : MonoBehaviour
{
    public static InputMgr instance;

    [Serializable]
    public class ClickData
    {
        //public bool is


        /// <summary> ��ġŰ </summary>
        public string key;
        /// <summary> ��ġ �̺�Ʈ </summary>
        public Action touchAction;
    }

    /// <summary> ����ϴ� Ű ��� </summary>
    private List<ClickData> keyList = new List<ClickData>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        // �Է��� ���� ��� ����
        if (!Input.anyKey)
        {
            return;
        }

        //if (Input.GetKeyDown(KeyCode.None)
        //{
        //
        //}
    }

    #region ��ȯ
    /// <summary> string�� KeyCode�� ��ȯ </summary>
    /// <param name="key"></param>
    private KeyCode ConvertStringToKeyCode(string key)
    {
        return key switch
        {
            "A" => KeyCode.A,
            _ => KeyCode.None,
        };
    }
    #endregion ��ȯ
}
