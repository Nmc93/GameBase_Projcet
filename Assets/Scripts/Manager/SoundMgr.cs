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

    /// <summary> ���� �� ������ �ε�Ǵ� ���� �̸� �ε� </summary>
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
