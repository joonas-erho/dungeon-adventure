using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 0)]
public class ItemScriptableObject : ScriptableObject {
    public string itemName;
    public Sprite sprite;
}