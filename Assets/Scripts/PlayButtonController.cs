using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonController : MonoBehaviour
{
    public GameController gameController;

    void OnMouseDown() {
        gameController.Execute();
    }
}
