using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Redpenguin.GoogleSheets.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Redpenguin.GoogleSheets.Scripts.Editor.Core
{
  public class GoogleSheetsProviderWindow : EditorWindow
  {
    [SerializeField] private VisualTreeAsset tree;
    [SerializeField] private VisualTreeAsset containerView;

    private GoogleSheetsProviderService _googleSheetsProviderService;
    private bool _isCreatingScripts;

    [MenuItem("GoogleSheets/Provider", false, 1)]
    private static void CreateWindows()
    {
      GetWindow<GoogleSheetsProviderWindow>("Google Sheets Provider").Show();
    }
    
    private void OnEnable()
    {
      _googleSheetsProviderService ??= new GoogleSheetsProviderService();
      _googleSheetsProviderService.FindAllContainers();
      AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
    }

    private void OnDisable()
    {
      AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
    }

    private void CreateGUI()
    {
      tree.CloneTree(rootVisualElement);

      rootVisualElement.Q<Button>("ButtonCreateSO").clickable.clicked += () =>
      {
        _isCreatingScripts = _googleSheetsProviderService.CreateAdditionalScripts();
      };
      rootVisualElement.Q<Button>("ButtonClear").clickable.clicked += _googleSheetsProviderService.Clear;
      rootVisualElement.Q<Button>("ButtonLoad").clickable.clicked += _googleSheetsProviderService.LoadSheetsData;
      rootVisualElement.Q<Button>("ButtonSave").clickable.clicked += _googleSheetsProviderService.SaveToFile;
      rootVisualElement.Q<Button>("ButtonSettings").clickable.clicked += GoogleSheetsProviderAssetMenu.SelectSettingsAsset;

      var folder = rootVisualElement.Q<VisualElement>("Containers");
      for (var i = 0; i < _googleSheetsProviderService.SpreadSheetContainers.Count; i++)
      {
        CreateGroupButton(i, folder);
      }
    }

    private void CreateGroupButton(int i, VisualElement folder)
    {
      var sheetEditorView = new SheetEditorView();
      var sheetEditorVisualElement = containerView.Instantiate();
      
      sheetEditorView.SetVisualElement(sheetEditorVisualElement);
      sheetEditorView.Add(_googleSheetsProviderService.SpreadSheetContainers[i],
        _googleSheetsProviderService.Settings.serializationGroups);
      sheetEditorVisualElement.userData = sheetEditorView;
      folder.Add(sheetEditorVisualElement);
    }

    private async void OnAfterAssemblyReload()
    {
      if (_isCreatingScripts == false) return;
      _isCreatingScripts = false;
      _googleSheetsProviderService.CreateScriptableObjects();
      await Task.Delay(TimeSpan.FromSeconds(0.1f));
      _googleSheetsProviderService.FindAllContainers();

      rootVisualElement.Clear();
      CreateGUI();
    }

    
  }
}