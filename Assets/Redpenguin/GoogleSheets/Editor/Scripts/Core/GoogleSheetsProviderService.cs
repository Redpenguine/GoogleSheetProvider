using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Redpenguin.GoogleSheets.Editor;
using Redpenguin.GoogleSheets.Editor.Utils;
using Redpenguin.GoogleSheets.Scripts.Editor.Utils;
using Redpenguin.GoogleSheets.Scripts.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Scripts.Editor.Core
{
  public class GoogleSheetsProviderService
  {
    public List<ScriptableObject> SpreadSheetContainers { get; private set; } = new();
    public GoogleSheetProviderSettings Settings { get; set; }

    private readonly DataImporter _dataImporter;
    private readonly SpreadSheetCodeFactory _codeFactory;
    private readonly SpreadSheetScriptableObjectFactory _scriptObjFactory;
    
    private ConfigDatabaseScriptableObject _configDatabase;
    public GoogleSheetsProviderService()
    {
      if (SetupSettings()) return;

      var googleSheetsReader = new GoogleSheetsReader(Settings.googleSheetID, Settings.credential.text);
      _dataImporter = new DataImporter(googleSheetsReader);
      _codeFactory = new SpreadSheetCodeFactory();
      _scriptObjFactory = new SpreadSheetScriptableObjectFactory();
    }

    public void FindAllContainers()
    {
      SpreadSheetContainers.Clear();
      
      AssetDatabaseHelper
        .FindAssetsByType<SpreadSheetWrapper>()
        .ForEach(x => SpreadSheetContainers.Add(x));
      
      CreateConfigDatabase();
    }

    private bool SetupSettings()
    {
      var providerSettingsList = AssetDatabaseHelper.FindAssetsByType<GoogleSheetProviderSettings>();
      switch (providerSettingsList.Count)
      {
        case > 1:
          Debug.LogError($"Find {providerSettingsList.Count} GoogleSheetProviderSettings. Remove all except 1.");
          break;
        case 0:
          Debug.LogError(
            $"Cant find GoogleSheetProviderSettings. Create via CreateAssetMenu -> Create -> GoogleSheets -> Settings.");
          return true;
      }

      Settings = providerSettingsList.First();
      return false;
    }
    
    public void Clear()
    {
      Caching.ClearCache();
      PlayerPrefs.DeleteAll();
      var di = new DirectoryInfo(GoogleSheetsVariables.SavePaths.DATABASE_SAVEPATH);
      if (!di.Exists) return;
      foreach (var file in di.EnumerateFiles())
      {
        Debug.Log($"{file.Name} delete");
        file.Delete();
      }
    }
    private void CreateConfigDatabase()
    {
      if (!SpreadSheetContainers.Any() || SpreadSheetContainers.Contains(null)) return;
      if (_configDatabase == null)
      {
        var configDatabases = AssetDatabaseHelper.FindAssetsByType<ConfigDatabaseScriptableObject>();
        if (configDatabases.Count == 0)
        {
          _scriptObjFactory.CreatConfigDatabase(_codeFactory.GetConfigDatabaseType());
          _configDatabase = AssetDatabaseHelper.FindAssetsByType<ConfigDatabaseScriptableObject>()[0];
        }
        else
        {
          _configDatabase = configDatabases[0];
        }
      }

      _configDatabase.AddContainers(SpreadSheetContainers);
      EditorUtility.SetDirty(_configDatabase);
    }
    public void LoadSheetsData()
    {
      _dataImporter.LoadSheetsSOData();
      CreateConfigDatabase();
      SaveToFile();
    }
    
    public bool CreateAdditionalScripts()
    {
      _scriptObjFactory.DeleteAllAssets();
      return _codeFactory.CreateAdditionalScripts();
    }

    public void CreateScriptableObjects()
    {
      _scriptObjFactory.CreateScriptableObjects(_codeFactory.GetGeneratedScriptsTypes());
    }
    public void SaveToFile()
    {
      ((ISpreadSheetSave) _configDatabase).SaveToFile();
      Debug.Log("ConfigDatabase save to file!");
      AssetDatabase.Refresh();
    }
  }
}