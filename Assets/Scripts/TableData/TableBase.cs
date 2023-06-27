using System.Collections;
using System.Collections.Generic;

public abstract class TableBase
{
    public abstract string TableName { get; }

    public abstract object GetKey { get; }
}
