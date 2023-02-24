using System.Collections.Generic;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  public interface ISpreadSheetSO
  {
    public string SerializationGroupTag { get; set; }
    string JsonSerialized { get; }
    ISheetDataContainer SheetDataContainer { get; }
    void SetListCount(int count);
  }
}