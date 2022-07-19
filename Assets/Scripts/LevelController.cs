using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int levelId;
    public PlayerController playerController;
    public MonsterController[] monsterControllers;
    public ItemController[] itemControllers;
    public DoorController doorController;

    // Actions that can be used in this level.
    public List<ActionScriptableObject> actions = new();
}
