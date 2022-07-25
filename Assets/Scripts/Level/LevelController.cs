using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls each individual level. The scene of the game never changes, but instead the
/// GameController loads a new level, which is controlled by an instance of this class.
/// </summary>
public class LevelController : MonoBehaviour
{
    // ID of this level. (first level is 0)
    public int levelId;

    // References to the objects inside of this level. Manually set in the Unity Editor.
    public PlayerController playerController;
    public List<MonsterController> monsterControllers;
    public ItemController[] itemControllers;
    public DoorController doorController;
    public int maxActionsForMaxPoints;

    // Actions that can be used in this level.
    public List<ActionScriptableObject> actions = new();

    /// <summary>
    /// This is called by the GameController as a new level is loaded to set up required references.
    /// </summary>
    /// <param name="gc"></param>
    /// <param name="pc">NOTE: Passed by reference.</param>
    /// <param name="dc">NOTE: Passed by reference.</param>
    public void SetConnections(GameController gc, out PlayerController pc, out DoorController dc) {
        playerController.gameController = gc;
        doorController.gameController = gc;
        pc = playerController;
        dc = doorController;
    }
}
