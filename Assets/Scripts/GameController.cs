using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Readonly values.
    private readonly float actionButtonGap = 0.8f;
    private readonly int amountOfActionsInRow = 11;

    // Prefab for the Action boxes.
    public GameObject actionPrefab;

    // The field that holds all the Actions that are available in this level.
    public GameObject actionStore;

    // The field that the player can drag Actions to.
    public GameObject actionQueue;

    // List of all actions. (Used mainly editor-side)
    public List<ActionScriptableObject> actions = new();

    // List of actions in the queue.
    private List<ActionController> queuedActions = new();
    

    void Start() {
        GenerateAvailableActions(actions);
    }

    /// <summary>
    /// Generates available actions and places them as children to the "Action Store" gameobject.
    /// </summary>
    /// <param name="list">List of actions as premade scriptable objects. (See Assets/Objects-folder.)</param>
    void GenerateAvailableActions(List<ActionScriptableObject> list) {
        for (int i = 0; i < list.Count; i++) {
            CreateAction(list[i], actionStore, new Vector2((i % amountOfActionsInRow) * actionButtonGap, (i / amountOfActionsInRow) * -actionButtonGap));
        }
    }

    private GameObject CreateAction(ActionScriptableObject action, GameObject parent, Vector2 position) {
        // Create game object and add as child to action store in correct position.
        GameObject go = Instantiate(actionPrefab);
        go.transform.SetParent(parent.transform);
        go.transform.localPosition = position;

        // Set correct name (visible in editor only).
        go.name = "Action (" + action.actionName + ")";
        
        // Set the object of the created prefab as the one given in the list.
        // See ActionController for more information.
        go.GetComponent<ActionController>().SetValues(action, this);

        return go;
    }

    public void AddActionToQueue(ActionScriptableObject action) {
        GameObject go =
            CreateAction(action,
                         actionQueue,
                         new Vector2((queuedActions.Count % amountOfActionsInRow) * actionButtonGap, (queuedActions.Count / amountOfActionsInRow) * -actionButtonGap));
        queuedActions.Add(go.GetComponent<ActionController>());
    }
}
