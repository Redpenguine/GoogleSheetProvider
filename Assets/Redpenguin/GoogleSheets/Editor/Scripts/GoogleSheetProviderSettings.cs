using System.Collections.Generic;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Editor
{
  [CreateAssetMenu(fileName = "GoogleSheetProviderSettings", menuName = "GoogleSheetProvider/Settings")]
  public class GoogleSheetProviderSettings : ScriptableObject
  {
    public string googleSheetID;
    public TextAsset credential;
    public List<SerializationGroup> serializationGroups;
    private readonly string _defaultGroup = "Default";
    
    private void OnValidate()
    {
      if (serializationGroups is {Count: >= 0})
      {
        var contains = false;
        foreach (var rule in serializationGroups)
        {
          if (rule.tag == _defaultGroup)
          {
            contains = true;
            break;
          }
        }

        if (!contains)
        {
          serializationGroups.Insert(0,new SerializationGroup{tag = _defaultGroup, color = new Color32(63, 147, 60, 255)});
        }
      }
    }
  }
}