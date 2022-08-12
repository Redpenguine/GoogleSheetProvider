using System;
using System.Collections.Generic;
using System.Linq;
using Redpenguin.GoogleSheets.Scripts.Runtime.Attributes;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  [Serializable]
  public abstract class SpreadSheetsDatabase
  {
    public List<ISheetDataContainer> Containers { get; private set; } = new List<ISheetDataContainer>();

    public T GetContainer<T>() where T : ISheetDataContainer
    {
      foreach (var container in Containers.OfType<T>())
      {
        return container;
      }

      throw new ArgumentNullException();
    }

    public List<T> GetSpreadSheetData<T>() where T : ISheetData, new()
    {
      foreach (var container in Containers.OfType<SheetDataContainer<T>>())
      {
        return container.Data;
      }

      throw new ArgumentNullException();
    }
    public void SetData<T>(List<T> list) where T : ISheetData, new()
    {
      foreach (var container in Containers.OfType<SheetDataContainer<T>>())
      {
        container.Data = list;
      }
    }

    // public void RegisterContainers(List<T> dataContainers)
    // {
    //   this.Containers = dataContainers;
    //   AddAditional();
    // }
    //
    // protected virtual void AddAditional() { }

    protected void AddContainer(ISheetDataContainer container)
    {
      if (!Containers.Contains(container))
        Containers.Add(container);
    }
  }
}