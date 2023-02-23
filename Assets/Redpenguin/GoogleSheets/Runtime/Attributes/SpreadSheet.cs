using System;
using System.Collections.Generic;
using System.Linq;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Attributes
{
  [AttributeUsage(AttributeTargets.Class)]
  public class SpreadSheet : Attribute
  {
    public string Name;
    public string Range;

    public SpreadSheet(string name, string from = "A1", string to = "Z1000")
    {
      Name = name;
      Range = $"{Name}!{from}:{to}";
    }
  }
}