using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonController : MonoBehaviour
{
    public GameController gameController;

    // When player presses the Play button, the game controller tells the player to execute the actions.
    void OnMouseDown() {
        gameController.Execute();
    }
}
