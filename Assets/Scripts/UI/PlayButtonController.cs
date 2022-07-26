using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonController : MonoBehaviour
{
    public GameController gameController;

    public SpriteRenderer sr;

    public Sprite[] sprites;

    // When player presses the Play button, the game controller tells the player to execute the actions.
    void OnMouseDown() {
        if (!gameController.isRunning) {
            gameController.Execute();
            sr.sprite = sprites[1];
        }
        else {
            gameController.queueShouldBeStopped = true;
        }
    }

    public void StopRunning() {
        sr.sprite = sprites[0];
    }
}
