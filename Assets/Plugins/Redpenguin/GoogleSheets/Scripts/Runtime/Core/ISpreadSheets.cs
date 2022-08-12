using System.Collections.Generic;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Core
{
  public interface ISpreadSheets
  {
    void SetListCount(int count);

    List<object> GetData();
  }
}