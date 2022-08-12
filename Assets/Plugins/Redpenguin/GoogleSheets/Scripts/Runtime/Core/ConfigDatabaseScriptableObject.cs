using System.Collections.Generic;
using Redpenguin.GoogleSheets.Scripts.Runtime.Attributes;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  public abstract class ConfigDatabaseScriptableObject : ScriptableObject
  {
    [SerializeField] private List<ScriptableObject> containers = new List<ScriptableObject>();

    public List<ScriptableObject> Containers => containers;
    public List<T> GetSpreadSheetData<T>() where T : ISheetData, new()
    {
      foreach (var container in containers)
      {
        if (container is SpreadSheetScriptableObject<T> scriptableObject)
        {
          return scriptableObject.Data;
        }
      }
      return default;
    }

    public void AddContainers(List<ScriptableObject> newContainers)
    {
      containers.Clear();
      newContainers.ForEach(newContainer => containers.Add(newContainer));
    }
  }
}