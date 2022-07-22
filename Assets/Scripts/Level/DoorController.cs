using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the goal of each level, which is a door with 0-3 locks.
/// </summary>
public class DoorController : MonoBehaviour
{
    public GameController gameController;

    // Amount of keys that the players needs to collect and use on the door to pass the level.
    [Range(0,3)]
    public int amountOfKeysNeeded;

    // Different sprites for different amounts of locks.
    public Sprite[] doorSprites;

    public SpriteRenderer sr;

    /// <summary>
    /// This is called when the player is standing on the door and uses a key from inventory.
    /// </summary>
    /// <returns>How many keys are still needed (if returns 0, will trigger winning the level).</returns>
    public int UseKey() {
        amountOfKeysNeeded--;
        sr.sprite = doorSprites[amountOfKeysNeeded];
        return amountOfKeysNeeded;
    }
}