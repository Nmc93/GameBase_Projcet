using System.IO;
using UnityEditor;

public class AssetbundleMgr
{
    [MenuItem("Assets/AssetBundle Build")]
    public static void AssetBundleBuild()
    {
        string path = "Assets/AssetBundle";

        //해당 경로에 대상이 없을 경우
        if(!Directory.Exists(path))
        {
            //경로 생성
            Directory.CreateDirectory(path);
        }

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        EditorUtility.DisplayDialog("에셋번들 빌드", "에셋 번들 빌드 완료", "확인");
    }
}
