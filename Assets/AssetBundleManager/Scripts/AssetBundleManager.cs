using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// All possible bundle type
/// </summary>
public enum BundleType
{
    A,
    B
}

/// <summary>
/// class containing bundle and the relative type
/// </summary>
public class BundleDataBase
{
    public AssetBundle Bundle;
    public BundleType BundleType;

    public BundleDataBase(AssetBundle bundle, BundleType type)
    {
        Bundle = bundle;
        BundleType = type;
    }
}

public static class AssetBundleManager
{
    /// <summary>
    /// Asset bundle directory
    /// </summary>
    public const string assetBundleDirectory = "Assets/StreamingAssets/AssetBundles";
    /// <summary>
    /// Fixed bundle name
    /// </summary>
    private const string bundleName = "/AssetBundles/mybundle";
    /// <summary>
    /// fixed bundle name
    /// </summary>
    private const string bundleNameB = "/AssetBundles/mybundleB";

    /// <summary>
    /// List all all bundle
    /// </summary>
    private static List<BundleDataBase> m_bundleDatabase = new List<BundleDataBase>(); 

    /// <summary>
    /// Load a object from bundle
    /// </summary>
    /// <param name="_bundleType">bundle type</param>
    /// <param name="objectName">prefab name</param>
    /// <returns></returns>
    public static Object LoadFromBundle(BundleType _bundleType, string objectName)
    {
        BundleDataBase currentDatabase = m_bundleDatabase.Find((x) => x.BundleType == _bundleType);

        if (currentDatabase == null)
        {
            try {
                AssetBundle loadedAssetBundle = GetCorrectBundleType(_bundleType);
                currentDatabase = new BundleDataBase(loadedAssetBundle, _bundleType);
                m_bundleDatabase.Add(currentDatabase);
            }
            catch (System.NullReferenceException)
            {
                Debug.LogError("AssetBundle load failed");
            }
        }
        Object obj = currentDatabase.Bundle.LoadAsset(objectName);
        if(obj == null)
        {
            Debug.Log("Fail to load " + objectName);
            return null;
        }
        return obj;
    }

    /// <summary>
    /// Load a object from bundle and return the corret type
    /// </summary>
    /// <typeparam name="T">Component type</typeparam>
    /// <param name="_bundleType">bundle type</param>
    /// <param name="objectName">prefab name</param>
    /// <returns>component type</returns>
    public static T LoadFromBundle<T>(BundleType _bundleType, string objectName) where T : Component
    {
        BundleDataBase currentDatabase = m_bundleDatabase.Find((x) => x.BundleType == _bundleType);
        if (currentDatabase == null)
        {
            try
            {
                AssetBundle loadedAssetBundle = GetCorrectBundleType(_bundleType);
                currentDatabase = new BundleDataBase(loadedAssetBundle, _bundleType);
                m_bundleDatabase.Add(currentDatabase);
            }
            catch (System.NullReferenceException)
            {
                Debug.LogError("AssetBundle load failed");
            }
        }
        GameObject temp = (GameObject)currentDatabase.Bundle.LoadAsset(objectName);
        if (temp == null)
        {
            Debug.LogError("Fail to load " + objectName);
            return null;
        }
        return temp.GetComponent<T>();
    }

    /// <summary>
    /// Unload a specific bundle
    /// </summary>
    /// <param name="_type">bundle type</param>
    private static void UnloadBundle(BundleType _type)
    {
        ///Unload all database
        m_bundleDatabase.Find((x) => x.BundleType == _type).Bundle.Unload(false);
    }

    /// <summary>
    /// Clear database
    /// </summary>
    private static void ShutDownAll()
    {
        ///Unload all database
        m_bundleDatabase.ForEach((x) => x.Bundle.Unload(false));
        ///clear list
        m_bundleDatabase.Clear();
    }

    /// <summary>
    /// check the association with the bundle type with the relative bundle name
    /// </summary>
    /// <param name="_bundleType"></param>
    /// <returns></returns>
    private static AssetBundle GetCorrectBundleType(BundleType _bundleType)
    {
        switch (_bundleType)
        {
            case BundleType.A:
                return AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath + bundleName));
            case BundleType.B:
                return AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath + bundleNameB));
            default:
                return AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath + bundleName));
        }
    }
}
