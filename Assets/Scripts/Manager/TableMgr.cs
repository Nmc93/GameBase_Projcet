using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMgr : MgrBase
{
    public static TableMgr instance;

    private void Awake()
    {
        instance = this;
    }
}
