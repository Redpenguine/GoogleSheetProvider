using UnityEngine;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  public abstract class ConfigurationScriptableObjectDatabaseProvider
  {
    private ConfigDatabaseScriptableObject _configDatabaseScriptableObject;
    public ConfigDatabaseScriptableObject ConfigDatabaseScriptableObject
    {
      get
      {
        if (_configDatabaseScriptableObject != null) return _configDatabaseScriptableObject;
        _configDatabaseScriptableObject = Resources.Load<ConfigDatabaseScriptableObject>(GoogleSheetsVariables.ConfigDatabasePath);
        if (_configDatabaseScriptableObject == null)
        {
          throw new System.Exception("ConfigDatabase doesn't exist!");
        }
        return _configDatabaseScriptableObject;
      }
    }

    // public async UniTask Load()
    // {
    //   if (_configDatabaseScriptableObject != null) return;
    //   var temp = await Resources.LoadAsync<ConfigDatabaseScriptableObject>(GoogleSheetsVariables.ConfigDatabasePath).ToUniTask();
    //   //await temp;
    //   _configDatabaseScriptableObject = temp as ConfigDatabaseScriptableObject;
    // }
  }
  public abstract class ConfigurationDatabaseProvider
  {
    private ConfigDatabaseScriptableObject _configDatabaseScriptableObject;
    public ConfigDatabaseScriptableObject ConfigDatabaseScriptableObject
    {
      get
      {
        if (_configDatabaseScriptableObject != null) return _configDatabaseScriptableObject;
        _configDatabaseScriptableObject = Resources.Load<ConfigDatabaseScriptableObject>(GoogleSheetsVariables.ConfigDatabasePath);
        if (_configDatabaseScriptableObject == null)
        {
          throw new System.Exception("ConfigDatabase doesn't exist!");
        }
        return _configDatabaseScriptableObject;
      }
    }

    // public async UniTask Load()
    // {
    //   if (_configDatabaseScriptableObject != null) return;
    //   var temp = await Resources.LoadAsync<ConfigDatabaseScriptableObject>(GoogleSheetsVariables.ConfigDatabasePath).ToUniTask();
    //   //await temp;
    //   _configDatabaseScriptableObject = temp as ConfigDatabaseScriptableObject;
    // }
  }
}