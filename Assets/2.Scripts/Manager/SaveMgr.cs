using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMgr : MgrBase
{
    public static SaveMgr instance;

    [Header("�ִ� ����� ����")]
    [SerializeField] private int saveMaxCount = 3;

    public override void Init()
    {
        instance = this;

    }
}
