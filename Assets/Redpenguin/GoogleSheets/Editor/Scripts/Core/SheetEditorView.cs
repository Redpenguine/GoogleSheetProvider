using System.Collections.Generic;
using Redpenguin.GoogleSheets.Editor;
using Redpenguin.GoogleSheets.Scripts.Runtime.Core;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Redpenguin.GoogleSheets.Scripts.Editor.Core
{
  public class SheetEditorView
  {
    public ObjectField ContainerObject;
    public VisualElement RulesContainer;
    private ISpreadSheet _spreadSheet;
    private readonly List<RuleButton> _buttons = new();

    public void Add(ScriptableObject scriptableObject, List<SerializationGroup> rules)
    {
      ContainerObject.SetValueWithoutNotify(scriptableObject);
      RulesContainer.Clear();
      _buttons.Clear();
      _spreadSheet = ((ISpreadSheet) scriptableObject);
      foreach (var rule in rules)
      {
        var rb = new Button(() => OnButtonClick(rule.tag));
        var ruleButton = new RuleButton(rb, rule);
        ruleButton.AddListener(OnButtonClick);
        rb.text = rule.tag;
        ruleButton.SetDarker(_spreadSheet.SerializationGroupTag == rule.tag ? 0 : 50);

        RulesContainer.Add(rb);
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
      
    public void SetVisualElement(VisualElement visualElement)
    {
      ContainerObject = visualElement.Q<ObjectField>("ContainerObject");
      RulesContainer = visualElement.Q<VisualElement>("RulesContainer");
    }
  }
}