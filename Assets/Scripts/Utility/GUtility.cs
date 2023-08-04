using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUtility
{
    #region 코루틴
    /// <summary> 시간을 키로 WaitForSeconds를 저장하는 딕셔너리 </summary>
    private static Dictionary<float, WaitForSeconds> dicWaitForSeconds = new Dictionary<float, WaitForSeconds>();
    
    /// <summary> 시간에 맞는 WaitForSeconds를 반환, 없으면 만들어서 줌 </summary>
    /// <param name="time"> 멈춰있는 시간 </param>
    public static WaitForSeconds GetWaitForSeconds(float time)
    {
        if(!dicWaitForSeconds.TryGetValue(time,out WaitForSeconds waitForSeconds))
        {
            waitForSeconds = new WaitForSeconds(time);
            dicWaitForSeconds.Add(time, waitForSeconds);
        }

        return waitForSeconds;
    }
    #endregion 코루틴

}
