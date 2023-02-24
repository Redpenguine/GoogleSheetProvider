using System;
using System.Linq;
using System.Threading.Tasks;
using Redpenguin.GoogleSheets.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

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

      ButtonActionLink();
      DropdownGroupsSetup();
      var folder = rootVisualElement.Q<VisualElement>("Containers");
      for (var i = 0; i < _googleSheetsProviderService.SpreadSheetContainers.Count; i++)
      {
        CreateGroupButton(i, folder);
      }
    }

    private void DropdownGroupsSetup()
    {
      var dropdownField = rootVisualElement.Q<DropdownField>("DropdownGroups");
      dropdownField.choices = _googleSheetsProviderService.Settings.serializationGroups.Select(x => x.tag).ToList();
      dropdownField.index = 0;
      dropdownField.Q(className:"unity-base-popup-field__text").style.backgroundColor = _googleSheetsProviderService.Settings.serializationGroups[0].color;
      dropdownField.RegisterValueChangedCallback(x => OnChangeDropdownValue(dropdownField));
    }

    private void OnChangeDropdownValue(DropdownField dropdownField)
    {
      var rule = _googleSheetsProviderService.Settings.serializationGroups[dropdownField.index];
      dropdownField.style.color = rule.color;
      dropdownField.Q(className:"unity-base-popup-field__text").style.backgroundColor = rule.color;
    }

    private void ButtonActionLink()
    {
      rootVisualElement.Q<Button>("ButtonCreateSO").clickable.clicked += () =>
      {
        _isCreatingScripts = _googleSheetsProviderService.CreateAdditionalScripts();
      };
      rootVisualElement.Q<Button>("ButtonClear").clickable.clicked += _googleSheetsProviderService.Clear;
      rootVisualElement.Q<Button>("ButtonLoad").clickable.clicked += _googleSheetsProviderService.LoadSheetsData;
      rootVisualElement.Q<Button>("ButtonSave").clickable.clicked += _googleSheetsProviderService.SaveToFile;
      rootVisualElement.Q<Button>("ButtonSettings").clickable.clicked += GoogleSheetsProviderAssetMenu.SelectSettingsAsset;
    }

    private void CreateGroupButton(int i, VisualElement folder)
    {
      var view = containerView.Instantiate();
      var containerSheetModel = new SheetEditorPresenter(
        view, 
        _googleSheetsProviderService.SpreadSheetContainers[i],  
        _googleSheetsProviderService.Settings.serializationGroups
        );
      view.userData = containerSheetModel;
      folder.Add(view);
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