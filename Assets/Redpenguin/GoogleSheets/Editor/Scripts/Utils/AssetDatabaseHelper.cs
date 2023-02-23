using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Editor.Utils
{
  public static class AssetDatabaseHelper
  {
    public static List<T> FindAssetsByType<T>() where T : Object
    {
      var assets = new List<T>();
      var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
      foreach (var guid in guids)
      {
        var assetPath = AssetDatabase.GUIDToAssetPath(guid);
        var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        if (asset != null)
        {
          assets.Add(asset);
        }
      }

      return assets;
    }
  }
}