/// <summary>
/// Reset Button Controller
/// Joonas Erho & Melinda Suvivirta, 31.07.2022
/// 
/// This class controls the reset button used to remove all actions from the queue.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonController : MonoBehaviour
{
    public GameController gameController;

    void OnMouseDown() {
        if (gameController.isRunning) {
            return;
        }
        
        gameController.ResetQueue();
    }
}
