/// <summary>
/// Scriptable Object: Action
/// Melinda Suvivirta, 31.07.2022
/// 
/// Used to easily create new actions from the Unity editor.
/// </summary>

using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Action", order = 1)]
public class ActionScriptableObject : ScriptableObject
{
  public string actionName;
  public Sprite icon;
  public Color color;
}
