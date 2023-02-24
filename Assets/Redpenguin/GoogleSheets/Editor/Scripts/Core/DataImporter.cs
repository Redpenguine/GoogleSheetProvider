using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Redpenguin.GoogleSheets.Editor.Utils;
using Redpenguin.GoogleSheets.Scripts.Runtime.Attributes;
using Redpenguin.GoogleSheets.Scripts.Runtime.Core;
using Redpenguin.GoogleSheets.Scripts.Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Scripts.Editor.Core
{
  public class DataImporter
  {
    private const string SheetsData = "SheetsData";
    private readonly GoogleSheetsReader _sheetsReader;

    public DataImporter(GoogleSheetsReader sheetsReader)
    {
      _sheetsReader = sheetsReader;
    }

    public void LoadAndLinkSheetsDataToSo(List<ScriptableObject> list)
    {
      //var databaseScriptObj = AssetDatabaseHelper.FindAssetsByType<SpreadSheetWrapper>();
      var databaseScriptObj = list;
      Debug.Log($"Count {databaseScriptObj.Count}");
      foreach (var database in databaseScriptObj)
      {
        var databaseType = database.GetType();
        var sheetValues = GetSheetValues(databaseType);
        var dataList = databaseType.GetFields().FirstOrDefault(x => (x.GetValue(database) is IList));
        if(dataList == null) return;
        ((ISpreadSheetSO) database).SetListCount(sheetValues.First().Value.Count);
        SetValues(dataList.GetValue(database) as IList, database, sheetValues);
        EditorUtility.SetDirty(database);
      }
    }

    private void SetValues(IList dataList, ScriptableObject database,
      IReadOnlyDictionary<string, List<object>> sheetValues)
    {
      //if (!(list.GetValue(database) is IList dataList)) return;
      for (var i = 0; i < dataList.Count; i++)
      {
        var dataClass = dataList[i];
        var dataClassFields = dataClass.GetType().GetFields();
        foreach (var field in dataClassFields)
        {
          var fieldName = field.Name;
          if (!sheetValues.ContainsKey(fieldName)) continue;
          if (sheetValues[fieldName].Count <= i) continue;

          var fieldData = sheetValues[fieldName][i];
          var isJson = IsJson(field.FieldType, fieldData);
          var isEnum = IsEnum(field.FieldType, fieldData);
          try
          {
            if (isJson != null)
            {
              field.SetValue(dataClass, isJson);
            }
            else if (isEnum != null)
            {
              field.SetValue(dataClass, isEnum);
            }
            else
            {
              field.SetValue(dataClass, Convert.ChangeType(sheetValues[fieldName][i], field.FieldType));
            }
          }
          catch
          {
            Debug.LogError($"Table {database.name}, field {field.Name} format isn't correct!");
            throw;
          }
        }
      }
    }

    private void SetValues(FieldInfo list, ISheetDataContainer database,
      IReadOnlyDictionary<string, List<object>> sheetValues)
    {
      if (!(list.GetValue(database) is IList dataList)) return;
      for (var i = 0; i < dataList.Count; i++)
      {
        var dataClass = dataList[i];
        var dataClassFields = dataClass.GetType().GetFields();
        foreach (var field in dataClassFields)
        {
          var fieldName = field.Name;
          if (!sheetValues.ContainsKey(fieldName)) continue;
          if (sheetValues[fieldName].Count <= i) continue;

          var fieldData = sheetValues[fieldName][i];
          var isJson = IsJson(field.FieldType, fieldData);
          var isEnum = IsEnum(field.FieldType, fieldData);
          if (isJson != null)
          {
            field.SetValue(dataClass, isJson);
          }
          else if (isEnum != null)
          {
            field.SetValue(dataClass, isEnum);
          }
          else
          {
            field.SetValue(dataClass, Convert.ChangeType(sheetValues[fieldName][i], field.FieldType));
          }
        }
      }
    }

    private Dictionary<string, List<object>> GetSheetValues(Type databaseType)
    {
      var spreadSheetRange = databaseType.GetAttributeValue((SheetRange st) => st.SpreadSheetRange);
      return _sheetsReader.GetValuesOnRange(spreadSheetRange);
    }

    private object IsJson(Type type, object value)
    {
      try
      {
        var result = JsonConvert.DeserializeObject(value.ToString(), type);
        return result;
      }
      catch
      {
        return null;
      }
    }

    private object IsEnum(Type type, object value)
    {
      try
      {
        var result = Enum.Parse(type, value.ToString());
        return result;
      }
      catch
      {
        return null;
      }
    }
  }
}