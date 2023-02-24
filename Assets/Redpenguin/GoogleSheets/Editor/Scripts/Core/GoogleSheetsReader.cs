﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using UnityEditor;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Scripts.Editor.Core
{
  public class GoogleSheetsReader : IDisposable

  {
    private static readonly string[] Scopes = {SheetsService.Scope.Spreadsheets};
    private static readonly string ApplicationName = PlayerSettings.productName;
    private static string _spreadsheetId;
    private static SheetsService _service;

    public GoogleSheetsReader(string spreadSheetId, string clientSecrets)
    {
      _spreadsheetId = spreadSheetId;
      var credential = GoogleCredential.FromJson(clientSecrets).CreateScoped(Scopes);
      _service = new SheetsService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName
      });
    }

    public Dictionary<string, List<object>> GetValuesOnRange(string range)
    {
      var request = _service.Spreadsheets.Values.Get(_spreadsheetId, range);
      request.MajorDimension = SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.COLUMNS;
      var values = request.Execute().Values;
      DebugLog(values);
      return values.ToDictionary(k => k.First().ToString(), list => list.ToList().GetRange(1, list.Count - 1));
    }

    private void DebugLog(IEnumerable<IList<object>> values)
    {
      var sb = new StringBuilder();
      foreach (var item in values)
      {
        foreach (var val in item)
        {
          sb.Append(val);
          sb.Append(" ");
        }

        Debug.Log(sb.ToString());
        sb.Clear();
      }
    }

    public void Dispose()
    {
      _service.Dispose();
    }
  }
}