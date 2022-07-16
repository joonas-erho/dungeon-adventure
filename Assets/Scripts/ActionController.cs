using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    // GameController that handles the whole scene.
    // This reference is added as actions are generated.
    private GameController gameController;

    // The scriptable object that dictates the look and functionality
    // of this action.
    private ActionScriptableObject action;

    // Sprite renderers of the action base and the icon, respectively.
    public SpriteRenderer sr;
    public SpriteRenderer iconSr;

    // On generation, set the look to match that of the scriptable object's.
    void Start() {
        sr.color = action.color;
        iconSr.sprite = action.icon;
    }

    public void SetValues(ActionScriptableObject action, GameController gc) {
        this.action = action;
        this.gameController = gc;
    }

    void OnMouseDown() {
        gameController.AddActionToQueue(action);
    }
}
