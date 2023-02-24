using Redpenguin.GoogleSheets.Runtime.Core;
using UnityEngine;

namespace Redpenguin.GoogleSheets.Scripts.Editor.Core
{
  [CreateAssetMenu(menuName = "GoogleSheetProvider/DefaultSerializationRule", fileName = "SerializationRule",
    order = 4)]
  public class DefaultSerializationRuleScriptableObject : SerializationRuleScriptableObject<DefaultSerializationRule>
  {
  }
}