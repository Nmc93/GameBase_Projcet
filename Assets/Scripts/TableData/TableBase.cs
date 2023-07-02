using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public abstract class TableBase
{
    public abstract string TableName { get; }

    public abstract object GetKey { get; }
}