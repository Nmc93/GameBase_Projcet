using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MgrBase
{
    SoundMgr instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        SetSound();
    }

    /// <summary> 시작 후 무조건 로드되는 사운드 미리 로드 </summary>
    public void SetSound()
    {

    }

    public static void PlaySound()
    {

    }

    public static void CloseSound()
    {

    }
}
