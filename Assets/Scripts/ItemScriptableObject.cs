using UnityEngine;

/// <summary>
/// Used to easily create new items from the Unity editor.
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 0)]
public class ItemScriptableObject : ScriptableObject {
    public string itemName;
    public Sprite sprite;
}