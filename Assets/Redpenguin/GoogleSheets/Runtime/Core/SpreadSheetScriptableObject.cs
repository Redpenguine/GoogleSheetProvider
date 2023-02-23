using System.Collections.Generic;
using Redpenguin.GoogleSheets.Scripts.Runtime.Attributes;
using UnityEngine.Serialization;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  public abstract class SpreadSheetScriptableObject<T> : SpreadSheetWrapper, ISpreadSheet where T : ISheetData, new()
  {
    public string serializationGroupTag = "Default";
    
    public List<T> data = new();
    public List<string> serializationRulesTag = new();
    public string SerializationGroupTag
    {
      get => serializationGroupTag;
      set => serializationGroupTag = value;
    }

    public void SetListCount(int count)
    {
      var result = count - data.Count;
      for (var i = 0; i < result; i++)
      {
        data.Add(new T());
      }
    }

    public List<object> GetData()
    {
      return data as List<object>;
    }
  }
  
}