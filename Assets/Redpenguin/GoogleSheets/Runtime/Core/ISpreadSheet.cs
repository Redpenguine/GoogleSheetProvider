using System.Collections.Generic;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  public interface ISpreadSheet
  {
    public string SerializationGroupTag { get; set; }
    void SetListCount(int count);

    List<object> GetData();
  }
}