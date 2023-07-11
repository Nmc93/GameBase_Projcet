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


        /// <summary> 터치키 </summary>
        public string key;
        /// <summary> 터치 이벤트 </summary>
        public Action touchAction;
    }

    /// <summary> 사용하는 키 목록 </summary>
    private List<ClickData> keyList = new List<ClickData>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        // 입력이 없을 경우 종료
        if (!Input.anyKey)
        {
            return;
        }

        //if (Input.GetKeyDown(KeyCode.None)
        //{
        //
        //}
    }

    #region 변환
    /// <summary> string을 KeyCode로 변환 </summary>
    /// <param name="key"></param>
    private KeyCode ConvertStringToKeyCode(string key)
    {
        return key switch
        {
            "A" => KeyCode.A,
            _ => KeyCode.None,
        };
    }
    #endregion 변환
}
