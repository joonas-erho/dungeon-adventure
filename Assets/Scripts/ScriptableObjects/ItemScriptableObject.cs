/// <summary>
/// Scriptable Object: Object
/// Joonas Erho, 31.07.2022
/// 
/// Used to easily create new objects from the Unity editor.
/// </summary>

using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 0)]
public class ItemScriptableObject : ScriptableObject {
    public string itemName;
    public Sprite sprite;
}