using System.Collections.Generic;
using Newtonsoft.Json;
using Redpenguin.GoogleSheets.Runtime.Core;
using Redpenguin.GoogleSheets.Scripts.Runtime.Attributes;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  public interface ISheetDataContainer<T> where T : ISheetData
  {
    public List<T> Data { get; }
  }
  public abstract class SpreadSheetScriptableObject<T> : SpreadSheetSoWrapper, ISheetDataContainer<T> where T : ISheetData, new()
  {
    public string serializationGroupTag = "Default";
    public override string JsonSerialized => JsonConvert.SerializeObject(SheetDataContainer);
    public override ISheetDataContainer SheetDataContainer => new SpreadSheetDataContainer<T>(data);
    public string Type => GetType().ToString();
    public List<T> data = new();
    public List<string> serializationRulesTag = new();
    public override string SerializationGroupTag
    {
      get => serializationGroupTag;
      set => serializationGroupTag = value;
    }
    [JsonIgnore] public List<T> Data => data;
    public override void SetListCount(int count)
    {
      var result = count - data.Count;
      for (var i = 0; i < result; i++)
      {
        data.Add(new T());
      }
    }

    [ContextMenuItem("Serialize", "Serialize")]
    public bool t;
    public void Serialize()
    {
      var config = new SpreadSheetsDatabase();
      
      JsonConverter[] converters = { new SpreadSheetsConverter()};
      config.AddContainer(SheetDataContainer);
      var t2 = JsonConvert.SerializeObject(config);
      var config2= JsonConvert.DeserializeObject<SpreadSheetsDatabase>(t2, new JsonSerializerSettings() { Converters = converters });
      Debug.Log(t2);
    }
    
  }
}