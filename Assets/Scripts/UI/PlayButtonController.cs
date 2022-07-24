using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonController : MonoBehaviour
{
    public GameController gameController;

    public SpriteRenderer sr;

    public Sprite[] sprites;

    private bool isRunning = false;

    // When player presses the Play button, the game controller tells the player to execute the actions.
    void OnMouseDown() {
        if (!isRunning) {
            gameController.Execute();
            sr.sprite = sprites[1];
        }
        else {
            gameController.queueShouldBeStopped = true;
            sr.sprite = sprites[0];
        }

        isRunning = !isRunning;
    }

    public void StopRunning() {
        isRunning = false;
        sr.sprite = sprites[0];
    }
}
