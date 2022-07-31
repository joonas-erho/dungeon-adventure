/// <summary>
/// Action Area
/// Joonas Erho, 31.07.2022
/// 
/// A common class for the queue and store controllers.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionArea : MonoBehaviour
{
    protected int amountOfActionsInRow = GameController.amountOfActionsInRow;
    protected float actionButtonGap = GameController.actionButtonGap;

    /// <summary>
    /// Creates a game object that represents an Action.
    /// </summary>
    /// <param name="action">The scriptable object that dictates the action's look and functionality.</param>
    /// <param name="parent">The game object that the created action will be a child to.</param>
    /// <param name="position">The position of the action respective to its parent's position.</param>
    /// <returns>The created GameObject.</returns>
    protected GameObject CreateAction(GameController gc, GameObject prefab, ActionScriptableObject action, GameObject parent, Vector2 position, bool isQueued) {
        // Create game object and add as child to action store in correct position.
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(parent.transform);
        go.transform.localPosition = position;

        // Set correct name (visible in editor only).
        go.name = "Action (" + action.actionName + ")";
        
        // Set the object of the created prefab as the one given in the list.
        // See ActionController for more information.
        go.GetComponent<ActionController>().SetValues(action, gc, isQueued);

        return go;
    }
}
