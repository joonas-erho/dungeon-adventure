using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Action", order = 1)]
public class ActionScriptableObject : ScriptableObject
{
  public string actionName;
  public Sprite icon;
  public Color color;

}
