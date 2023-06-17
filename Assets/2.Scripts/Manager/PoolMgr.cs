using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GEnum;

public class PoolMgr : MgrBase
{
    public static PoolMgr instance;

    private void Awake()
    {
        instance = this;
    }
}
