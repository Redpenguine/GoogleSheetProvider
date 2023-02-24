using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Redpenguin.GoogleSheets.Scripts.Runtime.Utils
{
  public class FileSerializer
  {
    //BINARY
    /// <summary>
    /// Writes the given object instance to a binary file.
    /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
    /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
    /// </summary>
    /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
    /// <param name="filePath">The file path to write the object instance to.</param>
    /// <param name="fileName">File name</param>
    /// <param name="objectToWrite">The object instance to write to the XML file.</param>
    /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
    public static void WriteToBinaryFile<T>(string filePath, string fileName, T objectToWrite, bool append = false)
    {
      if (!Directory.Exists(filePath))
      {
        Directory.CreateDirectory(filePath);
      }
      var path = Path.Combine(filePath, fileName);
      using (Stream stream = File.Open(path, append ? FileMode.Append : FileMode.Create))
      {
        var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        binaryFormatter.Serialize(stream, objectToWrite);
      }
    }
    
    public static void WriteToJsonFile<T>(string filePath, string fileName, T objectToWrite)
    {
      if (!Directory.Exists(filePath))
      {
        Directory.CreateDirectory(filePath);
      }
      var path = Path.Combine(filePath, fileName);
      using (StreamWriter file = File.CreateText(path))
      {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(file, objectToWrite);
      }
    }
  }
}