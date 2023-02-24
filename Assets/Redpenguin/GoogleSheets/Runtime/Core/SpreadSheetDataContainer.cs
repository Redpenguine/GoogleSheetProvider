using System;
using System.Collections.Generic;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  [Serializable]
  public class SpreadSheetDataContainer<T> : ISheetDataContainer where T : new()
  {
    public List<T> Data { get; set; }
    public string Type => GetType().ToString();

    public SpreadSheetDataContainer(List<T> data)
    {
      Data = data;
    }
  }
}