using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddressableManager : MgrBase
{
    public static AddressableManager instance;

    private void Awake()
    {
        instance = this;
    }


}
