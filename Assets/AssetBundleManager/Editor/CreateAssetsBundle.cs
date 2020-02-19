using UnityEditor;
using System.IO;

public class CreateAssetsBundle
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        if (!Directory.Exists(AssetBundleManager.assetBundleDirectory))
        {
            Directory.CreateDirectory(AssetBundleManager.assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(AssetBundleManager.assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);
    }
}
