using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Readonly values.
    private readonly float actionButtonGap = 0.89f;
    private readonly int amountOfActionsInRow = 10;

    // The time that the system waits until it executes the next action.
    public float timeBetweenActions = 0.5f;

    // Level management variables
    public GameObject[] levels;
    public int currentLevelIndex;
    public LevelController currentLevelController;

    // Prefab for the Action boxes.
    public GameObject actionPrefab;

    public PlayerController playerController;
    public DoorController doorController;

    // The field that holds all the Actions that are available in this level.
    public GameObject actionStore;

    // The field that the player can drag Actions to.
    public GameObject actionQueue;

    // These render the items while they are in the player's inventory.
    public SpriteRenderer[] inventoryRenderers = new SpriteRenderer[3];

    public PlayButtonController playButtonController;

    // List of all actions. (Used mainly editor-side)
    public List<ActionScriptableObject> actions = new();

    // List of actions in the queue.
    private List<ActionController> queuedActions = new();
    
    public bool isDead = false;

    void Start() {
        LoadLevel(currentLevelIndex);
        GenerateAvailableActions(actions);
    }

    private void LoadNewLevel(int index) {
        if (playerController != null) {
            ResetInventory();
        }
        if (currentLevelController != null) {
            RemoveCurrentLevel();
        }
        RemoveAllActions();
        LoadLevel(index);
    }

    public void ResetLevel() {
        ResetInventory();
        RemoveCurrentLevel();
        LoadLevel(currentLevelIndex);
    }

    private void RemoveCurrentLevel() {
        Destroy(currentLevelController.gameObject);
        currentLevelController = null;
    }

    private void LoadLevel(int index) {
        playButtonController.StopRunning();
        GameObject go = Instantiate(levels[index]);
        currentLevelController = go.GetComponent<LevelController>();
        currentLevelController.SetConnections(this, out playerController, out doorController);
        isDead = false;
    }

    private void RemoveAllActions() {
        ResetQueue();
    }

    private void ResetInventory() {
        foreach (var r in inventoryRenderers) {
            r.sprite = null;
        }
    }

    public void WinLevel() {
        currentLevelIndex++;
        LoadNewLevel(currentLevelIndex);
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
        if (queuedActions.Count >= 50) {
            return;
        }
        
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
        // Since ExecuteActions is a coroutine, we have to use StartCoroutine here.
        StartCoroutine(ActionLoop());
    }

    private IEnumerator ActionLoop() {
        List<string> words = new List<string>();
        foreach (var actionController in queuedActions) {
            words.Add(actionController.GetWord());
        }

        for (int i = 0; i < words.Count; i++) {
            playerController.Execute(words[i]);
            queuedActions[i].MakeActive(true);
            foreach (var monster in currentLevelController.monsterControllers) {
                monster.Execute(monster.commands[i % monster.commands.Count]);
            }
            if (isDead) {
                queuedActions[i].MakeActive(false);
                break;
            }
            yield return new WaitForSeconds(timeBetweenActions);
            queuedActions[i].MakeActive(false);
        }

        if (isDead) {
            ResetLevel();
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        ResetLevel();
    }
}
