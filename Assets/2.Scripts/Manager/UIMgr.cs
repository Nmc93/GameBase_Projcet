using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEnum;

public class UIMgr : MgrBase
{
    public static UIMgr instance;

    private void Awake()
    {
        instance = this;
    }
}
