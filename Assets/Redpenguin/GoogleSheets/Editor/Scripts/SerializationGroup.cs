using System;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Editor
{
  [Serializable]
  public class SerializationGroup
  {
    public string tag;
    public Color color;
    public string savePath;
    public string fileName;
    public bool packSeparately;
  }
}