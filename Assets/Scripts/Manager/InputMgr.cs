using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GEnum;

public class InputMgr : MgrBase
{
    public static InputMgr instance;

    [Serializable]
    public class ClickData
    {
        public ClickData(KeyCode key)
        {
            this.key = key;
            touchAction = null;
        }

        /// <summary> ��ġŰ </summary>
        public KeyCode key;
        /// <summary> ��ġ �̺�Ʈ </summary>
        public Action touchAction;
        /// <summary> ��ġ �̺�Ʈ ��� </summary>
        public HashSet<Action> Actions = new HashSet<Action>();
    }

    /// <summary> ����ϴ� Ű ��� </summary>
    private static List<ClickData> keyList = new List<ClickData>();
    /// <summary> ��ɿ� �Ҵ�� Ű Ȯ�� </summary>
    private static Dictionary<eInputType, KeyCode?> dicInUseData = new Dictionary<eInputType, KeyCode?>();
    
    #region ���� ����
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        //���� Ű ����
        KeySetting();
    }

    /// <summary> ����Ǿ� �ִ� ���·� Ű ���� </summary>
    private void KeySetting()
    {
        //Ű ����� �ʿ��Ѱ�
        // eInputType       - .Tostring()�� ���̺�� �������� Ű���� ����
        // InputKeyTable    - ����Ʈ Ű ���� ����
        // PlayerPrefs      - ������ Ű ������ ����, �����Ͱ� ���ٸ� ���̺����� ����� ���
        // InputKeyTable�� PlayerPrefs�� ����� ���� KeyCode.*.ToString()�� ����
        // �������� �ʾ� ������� �ʴ� ����� ���� Null�� ����

        //Ű ����
        int count = (int)eInputType.Count;
        for (int i = 0; i < count; ++i)
        {
            eInputType type = (eInputType)i;
            string typeName = type.ToString();

            //Ű�� ���� ��� ����Ʈ���� Ű ���� �� ����
            if (!PlayerPrefs.HasKey(typeName))
            {
                PlayerPrefs.SetString(typeName, TableMgr.Get<InputKeyTableData>(typeName).KeyString);
                PlayerPrefs.Save();
            }

            //�ش� Ű ����
            KeyCode? code = ConvertStringToKeyCode(PlayerPrefs.GetString(typeName));
            dicInUseData.Add(type, code);

            //�� ���� �ƴ϶�� ���Ű ��Ͽ� ����
            if(code != null)
            {
                keyList.Add(new ClickData(code.Value));
            }
        }
    }
    #endregion ���� ����

    private void Update()
    {
        // �Է��� ���� ��� ����
        if (!Input.anyKey)
        {
            return;
        }

        //Ű �˻�
        foreach(var data in keyList)
        {
            //��ư�� ������ ��ġ �̺�Ʈ�� ��ϵ� ��� ����
            if (Input.GetKeyDown(data.key) && data.Actions.Count > 0)
            {
                foreach(var action in data.Actions)
                {
                    action();
                }
            }
        }
    }

    #region Ű �̺�Ʈ ����

    /// <summary> Ű Ŭ�� �̺�Ʈ ��� </summary>
    public static void AddKeyEvent(eInputType type, Action callback)
    {
        //Ÿ�Կ� ������ Ű �˻�
        if(dicInUseData.TryGetValue(type, out KeyCode? code) && code != null)
        {
            ClickData data = keyList.Find(item => item.key == code.Value);
            //������ Ű�� ���� ���
            if (data != null)
            {
                //���� ��ϵ��� ���� �̺�Ʈ�� ���
                if (!data.Actions.Contains(callback))
                {
                    data.Actions.Add(callback);
                }
                else
                {
                    Debug.LogError($"{type} �̺�Ʈ�� �ߺ� ����ǰ� �ֽ��ϴ�.");
                }
            }
            else
            {
                Debug.LogError($"{type}�� ������ Ű�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError($"{type}Ÿ���� �����͸� ã�� �� �����ϴ�.");
        }
    }

    /// <summary> Ű Ŭ�� �̺�Ʈ ���� </summary>
    public static void RemoveKeyEvent(eInputType type, Action callback)
    { 
        //Ÿ�Կ� ������ Ű �˻�
        if (dicInUseData.TryGetValue(type, out KeyCode? code) && code != null)
        {
            //������ Ű�� ���� ��� �̺�Ʈ ����
            ClickData data = keyList.Find(item => item.key == code.Value);
            if (data != null)
            {
                //��ϵ� �̺�Ʈ�� ���
                if (data.Actions.Contains(callback))
                {
                    data.Actions.Remove(callback);
                }
                //��ϵ��� ���� �̺�Ʈ�� ���
                else
                {
                    Debug.LogError($"������ {type}Ÿ���� �����͸� ã�� �� �����ϴ�.");
                }
            }
            else
            {
                Debug.LogError($"{type}�� ������ Ű�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError($"{type}Ÿ���� �����͸� ã�� �� �����ϴ�.");
        }
    }

    #endregion Ű �̺�Ʈ ����

    #region ��ȯ
    /// <summary> string�� KeyCode�� ��ȯ </summary>
    /// <param name="key"> KeyCode�� �׸�� �̸��� �����ؾ���</param>
    /// <returns> ã�� �� ���ٸ� null ��ȯ </returns>
    private KeyCode? ConvertStringToKeyCode(string key)
    {
        if(key == "Null")
        {
            return null;
        }

        try
        {
            KeyCode code = (KeyCode)Enum.Parse(typeof(KeyCode), key);
            return code;
        }
        catch (Exception ex)
        {
            Debug.LogError($"KeyCode ��ȯ ���� : [{ex.Message}], Key : [{key}]");
            return null;
        }
    }
    #endregion ��ȯ
}