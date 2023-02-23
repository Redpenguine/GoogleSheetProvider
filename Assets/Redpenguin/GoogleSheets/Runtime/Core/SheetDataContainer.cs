using System;
using System.Collections.Generic;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  [Serializable]
  public abstract class SheetDataContainer<T> : ISheetDataContainer where T : new()
  {
    public List<T> Data { get; set; } = new List<T>();

    public void Add(T data)
    {
      Data.Add(data);
    }

    public void CountSetup(int count)
    {
      var diff = count - Data.Count;
      if(diff <= 0) return;
      var amount = Data.Count;
      for (var i = 0; i < diff; i++)
      {
        Data.Add(ClassSetup(i + amount + 1));
      }
    }

    protected virtual T ClassSetup(int id)
    {
      return new T();
    }
  }
}