using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class UnityExtensions
{
}

public static class ObjectExtensionEditor
{
    public static string GetAssetPath(this UnityEngine.Object i_this)
    {
        if (i_this == null)
        {
            return string.Empty;
        }

        return AssetDatabase.GetAssetPath(i_this.GetInstanceID());
    }

    public static string GetAssetGUID(this UnityEngine.Object i_this)
    {
        if (i_this == null)
        {
            return string.Empty;
        }

        var assetPath = i_this.GetAssetPath();
        var guid = AssetDatabase.AssetPathToGUID(assetPath);
        return guid;
    }
}
