using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonController : MonoBehaviour
{
    public GameController gameController;

    void OnMouseDown() {
        gameController.ResetQueue();
    }
}
