using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueController : ActionArea
{
    // List of actions in the queue.
    private List<ActionController> queuedActions = new();

    public int GetActionCount() {
        return queuedActions.Count;
    }

    public List<ActionController> GetActions() {
        return queuedActions;
    }

    /// <summary>
    /// Adds an action to the queue by creating a new action and adding it to the queued actions list.
    /// </summary>
    /// <param name="action">The scriptable object to create the game object from.</param>
    public void AddActionToQueue(ActionScriptableObject action, GameController gc, GameObject prefab) {
        if (queuedActions.Count >= 50) {
            return;
        }
        
        GameObject go = CreateAction(
            gc,
            prefab,
            action,
            this.gameObject,
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

    public void RefreshQueue() {
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
}
