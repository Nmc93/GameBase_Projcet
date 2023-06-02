using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MgrBase : MonoBehaviour
{
    /// <summary> 시작시 사용되는 함수 </summary>
    public abstract void Init();

    /// <summary> 갱신시 사용되는 함수 </summary>
    public virtual void Refresh() { }
}