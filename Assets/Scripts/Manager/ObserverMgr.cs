using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

public class ObserverMgr : MgrBase
{
    public static ObserverMgr instance;

    private void Awake()
    {
        instance = this;
    }
}
