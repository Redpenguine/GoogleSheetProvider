using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Redpenguin.GoogleSheets.Editor
{
  [Serializable]
  public class GoogleSheetMetaData
  {
    public List<SheetContainerMetaData> sheetContainerMetaData;
    public SheetContainerMetaData Get(string containerType)
    {
      var data = sheetContainerMetaData.Find(x => x.containerType == containerType);
      if (data == null)
      {
        data = new SheetContainerMetaData {containerType = containerType};
        sheetContainerMetaData.Add(data);
      }
      return data;
    }
  }
  public interface IMetaData {}
  [Serializable]
  public class SheetContainerMetaData : IMetaData
  {
    public string containerType;
    public string savePath;
    public string fileName;
  }
  [CreateAssetMenu(fileName = "GoogleSheetProviderSettings", menuName = "GoogleSheetProvider/Settings")]
  public class GoogleSheetProviderSettings : ScriptableObject
  {
    public string googleSheetID;
    public TextAsset credential;
    public List<SerializationGroup> serializationGroups;

    public SerializationGroup defaultGroup;
    
    [HideInInspector] 
    public SerializationGroup currentGroup;

    public GoogleSheetMetaData googleSheetMetaData;
    public List<SerializationGroup> SerializationGroups => GetSerializationGroups();

    private List<SerializationGroup> GetSerializationGroups()
    {
      var list = new List<SerializationGroup>(serializationGroups);
      if (list.Count == 0)
      {
        list.Add(defaultGroup);
      }
      else
      {
        list.Insert(0,defaultGroup);
      }
        
      return list;
    }
  }
}