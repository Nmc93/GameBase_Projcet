using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMgr : MgrBase
{
    public static KeyboardMgr instance;

    public class ClickData
    {
        string key;

        bool isKeyDown;
    }

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

        if(Input.GetKeyDown(""))
        {

        }
    }
}
