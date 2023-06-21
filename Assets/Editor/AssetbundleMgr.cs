using System.IO;
using UnityEditor;

public class AssetbundleMgr
{
    [MenuItem("Assets/AssetBundle Build")]
    public static void AssetBundleBuild()
    {
        string path = "Assets/AssetBundle";

        //�ش� ��ο� ����� ���� ���
        if(!Directory.Exists(path))
        {
            //��� ����
            Directory.CreateDirectory(path);
        }

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        EditorUtility.DisplayDialog("���¹��� ����", "���� ���� ���� �Ϸ�", "Ȯ��");
    }
}
