using System;
using Redpenguin.GoogleSheets.Editor;
using UnityEngine.UIElements;

namespace Redpenguin.GoogleSheets.Scripts.Editor.Core
{
  public class RuleButton
  {
    private readonly Button _button;
    public readonly SerializationGroup Group;
      
    public RuleButton(Button button, SerializationGroup group)
    {
      _button = button;
      Group = group;
      _button.text = Group.tag;
    }
    public void AddListener(Action<string> onClick)
    {
      _button.clickable.clicked += () => onClick.Invoke(Group.tag);
    }

    public void SetDarker(int percent)
    {
      _button.style.backgroundColor = Group.color.MakeDarker(percent);
    }
  }
}