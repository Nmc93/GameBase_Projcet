using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleLoadMgr : MgrBase
{
    public static BundleLoadMgr instance;

    /// <summary> ���� ���� �н� </summary>
    private string path = "Assets/AssetBundle";

    /// <summary> ���¹��� �ε� �ڷ�ƾ </summary>
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
