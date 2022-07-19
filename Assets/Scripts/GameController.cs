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

    public PlayerController playerController;

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
            CreateAction(list[i], actionStore, new Vector2((i % amountOfActionsInRow) * actionButtonGap, (i / amountOfActionsInRow) * -actionButtonGap), false);
        }
    }

    /// <summary>
    /// Creates a game object that represents an Action.
    /// </summary>
    /// <param name="action">The scriptable object that dictates the action's look and functionality.</param>
    /// <param name="parent">The game object that the created action will be a child to.</param>
    /// <param name="position">The position of the action respective to its parent's position.</param>
    /// <returns>The created GameObject.</returns>
    private GameObject CreateAction(ActionScriptableObject action, GameObject parent, Vector2 position, bool isQueued) {
        // Create game object and add as child to action store in correct position.
        GameObject go = Instantiate(actionPrefab);
        go.transform.SetParent(parent.transform);
        go.transform.localPosition = position;

        // Set correct name (visible in editor only).
        go.name = "Action (" + action.actionName + ")";
        
        // Set the object of the created prefab as the one given in the list.
        // See ActionController for more information.
        go.GetComponent<ActionController>().SetValues(action, this, isQueued);

        return go;
    }

    /// <summary>
    /// Adds an action to the queue by creating a new action and adding it to the queued actions list.
    /// </summary>
    /// <param name="action">The scriptable object to create the game object from.</param>
    public void AddActionToQueue(ActionScriptableObject action) {
        GameObject go =
            CreateAction(action,
                         actionQueue,
                         new Vector2((queuedActions.Count % amountOfActionsInRow) * actionButtonGap, (queuedActions.Count / amountOfActionsInRow) * -actionButtonGap),
                         true);
        queuedActions.Add(go.GetComponent<ActionController>());
    }

    public void RemoveActionFromQueue(ActionController ac) {
        int index = queuedActions.FindIndex(i => i == ac);
        queuedActions.RemoveAt(index);
        Destroy(ac.gameObject);
        RefreshQueue();
    }

    private void RefreshQueue() {
        for (int i = 0; i < queuedActions.Count; i++) {
            GameObject go = queuedActions[i].gameObject;
            go.transform.localPosition = new Vector2((i % amountOfActionsInRow) * actionButtonGap, (i / amountOfActionsInRow) * -actionButtonGap);
        }
    }

    public void ResetQueue() {
        foreach (ActionController ac in queuedActions) {
            Destroy(ac.gameObject);
        }
        queuedActions.Clear();
    }

    /// <summary>
    /// Transforms the current queue of actions to strings and makes the player execute them.
    /// </summary>
    public void Execute() {
        List<string> words = new List<string>();
        foreach (var actionController in queuedActions) {
            words.Add(actionController.GetWord());
        }

        // Since ExecuteActions is a coroutine, we have to use StartCoroutine here.
        StartCoroutine(playerController.ExecuteActions(words));
    }

    public void StopExecute() {
        
    }
}
