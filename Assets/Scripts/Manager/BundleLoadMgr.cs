using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleLoadMgr : MgrBase
{
    public static BundleLoadMgr instance;

    /// <summary> 에셋 번들 패스 </summary>
    private string path = "Assets/AssetBundle";

    /// <summary> 에셋번들 로드 코루틴 </summary>
    private Coroutine LoadBundleBuild = null;

    private void Awake()
    {
        instance = this;
    }

    public void LoadBundle()
    {

    }

    private IEnumerator LoadAssetBundle()
    {

        yield return null;
    }

}
