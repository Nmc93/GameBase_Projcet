using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsMgr : MgrBase
{
    public static AssetsMgr instance;

    private void Awake()
    {
        instance = this;
    }

    /// <summary> 프리팹 기본 경로 </summary>
    private const string PrefabPath = "Prefab/";


    /// <summary> 경로를 받아서 UI 프리팹을 로드해서 반환 </summary>
    /// <param name="path"> 오브젝트 경로 </param>
    public static GameObject LoadResourcesUIPrefab(string path)
    {
        path = $"{PrefabPath}{path}";
        GameObject obj = Resources.Load<GameObject>(path);

        if (obj == null)
        {
            Debug.LogError($"잘못된 경로입니다. [{path}]");
        }

        return obj;
    }

    /// <summary> 경로를 받아서 UI 프리팹을 로드해서 반환 </summary>
    /// <param name="path"> 오브젝트 경로 </param>
    public static bool LoadResourcesUIPrefab(string path, out GameObject obj)
    {
        obj = Resources.Load<GameObject>(path);

        if (obj == null)
        {
            Debug.LogError($"잘못된 경로입니다. [{path}]");
        }

        return obj != null;
    }
}