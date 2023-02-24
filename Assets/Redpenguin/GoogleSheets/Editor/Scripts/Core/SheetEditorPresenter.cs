using System.Collections.Generic;
using Redpenguin.GoogleSheets.Editor;
using Redpenguin.GoogleSheets.Scripts.Runtime.Core;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Redpenguin.GoogleSheets.Scripts.Editor.Core
{
  public class SheetEditorPresenter
  {
    private ObjectField _containerObject;
    private VisualElement _rulesContainer;
    private ISpreadSheet _spreadSheet;
    private readonly List<RuleButton> _buttons = new();

    public SheetEditorPresenter(VisualElement view, ScriptableObject model, List<SerializationGroup> rules)
    {
      SetView(view);
      ModelViewLink(model, rules);
    }
    private void ModelViewLink(ScriptableObject scriptableObject, List<SerializationGroup> rules)
    {
      _containerObject.SetValueWithoutNotify(scriptableObject);
      _rulesContainer.Clear();
      _buttons.Clear();
      _spreadSheet = ((ISpreadSheet) scriptableObject);
      foreach (var rule in rules)
      {
        var button = new Button(() => OnButtonClick(rule.tag));
        var ruleButton = new RuleButton(button, rule);
        ruleButton.AddListener(OnButtonClick);
        ruleButton.SetDarker(_spreadSheet.SerializationGroupTag == rule.tag ? 0 : 50);

        _rulesContainer.Add(button);
        _buttons.Add(ruleButton);
      }
    }

    private void OnButtonClick(string tag)
    {
      foreach (var button in _buttons)
      {
        button.SetDarker(button.Group.tag == tag ? 0 : 50);
      }
      _spreadSheet.SerializationGroupTag = tag;
    }
      
    private void SetView(VisualElement visualElement)
    {
      _containerObject = visualElement.Q<ObjectField>("ContainerObject");
      _rulesContainer = visualElement.Q<VisualElement>("RulesContainer");
    }
  }
}