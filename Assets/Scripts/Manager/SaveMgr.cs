using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMgr : MgrBase
{
    public static SaveMgr instance;

    //[Header("최대 저장소 숫자")]
    //[SerializeField] private int saveMaxCount = 3;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }
}
