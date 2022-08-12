using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Redpenguin.GoogleSheets.Scripts.Editor.Utils;
using Redpenguin.GoogleSheets.Scripts.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Scripts.Editor.Core
{
  public class GoogleSheetsWindow : CustomEditorWindow
  {
    public string googleSheetId;
    public string credentialPath;
    public TextAsset credential;

    public List<ScriptableObject> _sheetsData = new List<ScriptableObject>();

    //public ConfigDatabaseScriptableObject _configDatabase;
    public ConfigDatabaseScriptableObject _configDatabase;

    private bool isShow = false;
    private GoogleSheetsReader _googleSheetsReader;
    private SerializedObject _serializedObject;
    private List<string> _list = new List<string>();
    private bool _isScriptsCreated = false;
    private SpreadSheetCodeFactory _codeFactory;
    private SpreadSheetScriptableObjectFactory _scriptObjFactory;
    private bool _foldout;
    private DataImporter _dataImporter;

    [MenuItem("Urmobi/Google Sheets", false, 3)]
    private static void CreateWindows()
    {
      GetWindow<GoogleSheetsWindow>("Google Sheets Provider").Show();
    }

    private void Awake()
    {
      //Debug.Log("Awake");
    }

    protected override void OnEnable()
    {
      //Debug.Log("OnEnable");
      base.OnEnable();
      SetSheetsDatesToEditorWindow();
      LoadCredentialAsset();
      CreateGoogleSheetParser();
      Initialization();
      AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
    }

    private void LoadCredentialAsset()
    {
      //if (credential != null) return;
      var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(credentialPath);
      if (asset != null)
      {
        credential = asset;
      }
    }

    private void OnValidate()
    {
      if (FieldIsEmpty()) return;
      credentialPath = AssetDatabase.GetAssetPath(credential);
      CreateGoogleSheetParser();
    }

    private void Initialization()
    {
      _codeFactory ??= new SpreadSheetCodeFactory();
      _scriptObjFactory ??= new SpreadSheetScriptableObjectFactory();
    }

    private void CreateGoogleSheetParser()
    {
      if (FieldIsEmpty()) return;
      _googleSheetsReader ??= new GoogleSheetsReader(googleSheetId, credential.text);
      _dataImporter ??= new DataImporter(_googleSheetsReader);
    }

    private bool FieldIsEmpty()
    {
      return credential == null || googleSheetId.Equals(string.Empty);
    }

    private bool SettingCheck()
    {
      if (FieldIsEmpty())
      {
        Debug.LogError("Google Sheet ID and Credential is empty!");
        return false;
      }

      return true;
    }

    protected override void OnDisable()
    {
      AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
      base.OnDisable();
    }

    private void SetSheetsDatesToEditorWindow()
    {
      _sheetsData.Clear();
      Resources.LoadAll<ScriptableObject>("SheetsData")
        .ToList().ForEach(x => _sheetsData.Add(x));
    }

    private void OnGUI()
    {
      _serializedObject = new SerializedObject(this);
      _foldout = EditorGUILayout.Foldout(_foldout, "Settings");
      if (_foldout)
      {
        googleSheetId = EditorGUILayout.TextField("Google Sheet ID", googleSheetId);
        ShowProperty("credential");
      }

      ShowProperty("_configDatabase");
      ShowList();
      SetButton("Create sheets SO", CreateAdditionalScripts);
      SetButton("Load data", LoadSheetsData);
      SetButton("Save to file", SaveToFile);
      SetButton("Clear all saves", ClearAll);
      //SetButton("OnAfterAssemblyReloadTest", CreateConfigDatabase);
      SetButton("Json", CreateJsonTest);

      _serializedObject.ApplyModifiedProperties();
    }

    private void ClearAll()
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

    private void SaveToFile()
    {
      ((ISpreadSheetSave) _configDatabase).SaveToFile();
      Debug.Log("ConfigDatabase save to file!");
      AssetDatabase.Refresh();
    }

    private static void CreateJsonTest()
    {
      var jsonSerializerSettings = new JsonSerializerSettings();
      jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
      //var result = JsonConvert.SerializeObject(ProductType.Consumable, jsonSerializerSettings);
      //var result2 = JsonConvert.DeserializeObject<ShopProduct>("{\"productType\" : \"Consumable\"}");
      //var result2 = JsonConvert.DeserializeObject<ShopProduct>("{\"Consumable\"}");
      //var result2 = JsonConvert.DeserializeObject<ProductType>("Consumable", jsonSerializerSettings);
      //Debug.Log(result + " " + result2);
    }

    private void SetButton(string label, Action action)
    {
      if (GUILayout.Button(label))
      {
        if (!SettingCheck()) return;
        action.Invoke();
      }
    }

    private void CreateConfigDatabase()
    {
      //AddContainers();
      if (!_sheetsData.Any() || _sheetsData.Contains(null)) return;
      _configDatabase ??= Resources.Load<ConfigDatabaseScriptableObject>(GoogleSheetsVariables.ConfigDatabasePath);
      if (_configDatabase == null)
      {
        _scriptObjFactory.CreatConfigDatabase(_codeFactory.GetConfigDatabaseType());
        _configDatabase = Resources.Load<ConfigDatabaseScriptableObject>(GoogleSheetsVariables.ConfigDatabasePath);
      }

      _configDatabase.AddContainers(_sheetsData);
    }

    private void AddContainers()
    {
      if (!_sheetsData.Any() || _sheetsData.Contains(null)) return;
      var database = Resources.Load<ConfigDatabaseScriptableObject>(GoogleSheetsVariables.ConfigDatabasePath);
      if (database != null)
      {
        _configDatabase = database;
      }

      ((ConfigDatabaseScriptableObject) _configDatabase).AddContainers(_sheetsData);
    }


    private void CreateAdditionalScripts()
    {
      _scriptObjFactory.DeleteAllAssets();
      _isScriptsCreated = _codeFactory.CreateAdditionalScripts();
    }

    private async void OnAfterAssemblyReload()
    {
      if (_isScriptsCreated == false) return;
      _isScriptsCreated = false;
      _scriptObjFactory.CreateScriptableObjects(_codeFactory.GetGeneratedScriptsTypes());
      await Task.Delay(TimeSpan.FromSeconds(0.1f));
      SetSheetsDatesToEditorWindow();
      CreateConfigDatabase();
    }

    private void LoadSheetsData()
    {
      _dataImporter.LoadSheetsSOData();
      CreateConfigDatabase();
      SaveToFile();
    }

    private void ShowList()
    {
      ShowProperty("_sheetsData");
    }

    private void ShowProperty(string property)
    {
      var stringsProperty = _serializedObject.FindProperty(property);
      EditorGUILayout.PropertyField(stringsProperty, true);
    }
  }

}