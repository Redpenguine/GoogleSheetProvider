using Newtonsoft.Json;
using Redpenguin.GoogleSheets.Runtime.Core;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  public class SpreadSheetsDatabaseProvider
  {
    public SpreadSheetsDatabase Database { get; private set; }

    public virtual void Load()
    {
      var serialization = new DefaultSerializationRule();
      var fileName = serialization.fileName;
      var fileTemplate = Resources.Load<TextAsset>($"{fileName}");
      if (fileTemplate != null)
      {
        
        Database = serialization.Deserialization<SpreadSheetsDatabase>(fileTemplate.text);
        Debug.Log($"<color=green>Load {fileName} from Resources/{fileName}</color>");
      }
      else
      {
        Database = new SpreadSheetsDatabase();
        Debug.Log("File template dont find!");
      }

      AdditionalLoad();
    }

    protected virtual void AdditionalLoad()
    {
    }
  }
}