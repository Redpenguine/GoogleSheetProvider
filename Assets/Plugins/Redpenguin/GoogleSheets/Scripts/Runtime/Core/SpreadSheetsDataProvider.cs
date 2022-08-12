using System.IO;
using Redpenguin.GoogleSheets.Scripts.Runtime.Utils;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  public abstract class SpreadSheetsDataProvider<T> where T : SpreadSheetsDatabase, new()
  {
    private const string Extension = ".bytes";
    private T _database;

    public SpreadSheetsDataProvider()
    {
      Load();
    }

    public T Database
    {
      get
      {
        if (_database != null) return _database;
        Load();
        if (_database == null)
        {
          throw new System.Exception($"{FileName} doesn't exist!");
        }

        return _database;
      }
    }

    protected virtual string FileName => "ConfigDatabaseTemplate";
    protected virtual string SavingDatabasePath => GoogleSheetsVariables.SavePaths.DATABASE_SAVEPATH;

    public virtual void Save()
    {
      var path = SavingDatabasePath;
      CreateIfNotExist(path);
      FileSerializer.WriteToBinaryFile($"{path}/{FileName}{Extension}", Database);
      AdditionalSave();
      Debug.Log($"<color=green>Save {FileName}</color>");
    }

    public virtual void Load()
    {
      var path = SavingDatabasePath;
      CreateIfNotExist(path);
      _database = new T();
      var fileTemplate = Resources.Load<TextAsset>($"{FileName}");
      if (fileTemplate != null)
      {
        _database = FileSerializer.Deserialize<T>(fileTemplate.bytes);
        Debug.Log($"<color=green>Load {FileName} from Resources/{FileName}{Extension}</color>");
      }
      else
      {
        Debug.Log("File template dont find!");
      }
      AdditionalLoad();
      InitAfterLoad();
    }

    private static void CreateIfNotExist(string path)
    {
      if (!Directory.Exists($"{path}"))
      {
        Directory.CreateDirectory($"{path}");
      }
    }

    protected virtual void AdditionalSave()
    {
    }

    protected virtual void AdditionalLoad()
    {
    }

    protected virtual void InitAfterLoad()
    {
    }
  }
}