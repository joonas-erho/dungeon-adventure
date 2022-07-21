using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to easily create new actions from the Unity editor.
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Action", order = 1)]
public class ActionScriptableObject : ScriptableObject
{
  public string actionName;
  public Sprite icon;
  public Color color;
}
