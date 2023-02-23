using System;
using Redpenguin.GoogleSheets.Editor;
using UnityEngine.UIElements;

namespace Redpenguin.GoogleSheets.Scripts.Editor.Core
{
  public class RuleButton
  {
    public Button Button;
    public SerializationGroup Group;
      
    public RuleButton(Button button, SerializationGroup group)
    {
      Button = button;
      Group = group;
    }
    public void AddListener(Action<string> onClick)
    {
      Button.clickable.clicked += () => onClick.Invoke(Group.tag);
    }

    public void SetDarker(int percent)
    {
      Button.style.backgroundColor = Group.color.MakeDarker(percent);
    }
  }
}