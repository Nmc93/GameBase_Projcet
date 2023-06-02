using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MgrBase : MonoBehaviour
{
    /// <summary> ���۽� ���Ǵ� �Լ� </summary>
    public abstract void Init();

    /// <summary> ���Ž� ���Ǵ� �Լ� </summary>
    public virtual void Refresh() { }
}