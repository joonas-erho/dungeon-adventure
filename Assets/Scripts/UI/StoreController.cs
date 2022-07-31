/// <summary>
/// Store Controller
/// Joonas Erho, 31.07.2022
/// 
/// This class controls the action store.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreController : ActionArea
{
    private List<GameObject> actionsInStore = new();

    /// <summary>
    /// Generates available actions and places them as children to this gameobject.
    /// </summary>
    /// <param name="list">List of actions as premade scriptable objects. (See Assets/Objects-folder.)</param>
    public void GenerateAvailableActions(List<ActionScriptableObject> list, float actionButtonGap, int amountOfActionsInRow, GameController gc, GameObject prefab) {
        // Clear actions if there are any.
        if (actionsInStore.Count > 0) {
            foreach (GameObject g in actionsInStore) {
                Destroy(g);
            }
            actionsInStore.Clear();
        }

        // Create new ones.
        for (int i = 0; i < list.Count; i++) {
            GameObject go = CreateAction(
                gc,
                prefab,
                list[i],
                this.gameObject,
                new Vector2(i * actionButtonGap, 0),
                false);
            
            actionsInStore.Add(go);
        }
    }
}
