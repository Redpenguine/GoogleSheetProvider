using System;

namespace Redpenguin.GoogleSheets.Runtime.Core
{
  [Serializable]
  public abstract class SerializationRule
  {
    public string filePath;
    public string fileName;
    public string extension;
    
    public abstract void Serialization(object objectToWrite);
    public abstract T Deserialization<T>(string text);
  }
}