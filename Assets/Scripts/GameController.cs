using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    // The gap between actions when they are placed into store or queue.
    public static readonly float actionButtonGap = 0.875f;

    // The amount of actions that fix on a horizontal row.
    public static readonly int amountOfActionsInRow = 10;

    // The time that the system waits until it executes the next action.
    public float timeBetweenActions = 0.5f;

    private int maxActionScore = 200;
    private int actionScoreLoss = 20;
    private int treasureValue = 50;
    private int monsterValue = 100;

    // Level management variables
    public GameObject[] levels;
    public int currentLevelIndex;
    public LevelController currentLevelController;

    // Prefab for the Action boxes.
    public GameObject actionPrefab;

    public PlayerController playerController;
    public DoorController doorController;

    // The controllers for the action zones.
    public StoreController storeController;
    public QueueController queueController;
    public VictoryScreenController victoryScreenController;

    // These render the items while they are in the player's inventory.
    public SpriteRenderer[] inventoryRenderers = new SpriteRenderer[3];

    public PlayButtonController playButtonController;

    // List of all actions. (Used mainly editor-side)
    public List<ActionScriptableObject> actions = new();
    
    public bool queueShouldBeStopped = false;
    public bool levelIsWon = false;

    public TextMeshProUGUI levelText;
    public int points;

    void Start() {
        LoadNewLevel(currentLevelIndex);
    }

    private void LoadNewLevel(int index) {
        if (playerController != null) {
            ResetInventory();
        }
        if (currentLevelController != null) {
            RemoveCurrentLevel();
        }
        ResetQueue();
        LoadLevel(index);
        storeController.GenerateAvailableActions(currentLevelController.actions, actionButtonGap, amountOfActionsInRow, this, actionPrefab);
        ShowLevelText(index);
    }

    public void ResetLevel() {
        victoryScreenController.gameObject.SetActive(false);
        ResetInventory();
        RemoveCurrentLevel();
        LoadLevel(currentLevelIndex);
    }

    private void RemoveCurrentLevel() {
        Destroy(currentLevelController.gameObject);
        currentLevelController = null;
    }

    private void LoadLevel(int index) {
        playButtonController.StopRunning();
        GameObject go = Instantiate(levels[index]);
        currentLevelController = go.GetComponent<LevelController>();
        currentLevelController.SetConnections(this, out playerController, out doorController);
        queueShouldBeStopped = false;
        levelIsWon = false;
    }

    private void ResetInventory() {
        foreach (var r in inventoryRenderers) {
            r.sprite = null;
        }
    }

    public void GoToNextLevel() {
        currentLevelIndex++;
        victoryScreenController.gameObject.SetActive(false);
        LoadNewLevel(currentLevelIndex);
    }

    public void WinLevel() {
        queueShouldBeStopped = true;
        levelIsWon = true;
        victoryScreenController.gameObject.SetActive(true);
        victoryScreenController.DisplayScore(CalculateActionScore(), CalculateTreasureScore(), CalculateMonsterScore());
    }

    public void ShowLevelText(int index) {
        levelText.text = "Level " + ++index;
    }

    /// <summary>
    /// Adds an action to the queue by creating a new action and adding it to the queued actions list.
    /// </summary>
    /// <param name="action">The scriptable object to create the game object from.</param>
    public void AddActionToQueue(ActionScriptableObject action) {
        queueController.AddActionToQueue(action, this, actionPrefab);
    }

    public void RemoveActionFromQueue(ActionController ac) {
        queueController.RemoveActionFromQueue(ac);
    }

    private void RefreshQueue() {
        queueController.RefreshQueue();
    }

    public void ResetQueue() {
        queueController.ResetQueue();
    }

    /// <summary>
    /// Transforms the current queue of actions to strings and makes the player execute them.
    /// </summary>
    public void Execute() {
        // Since ExecuteActions is a coroutine, we have to use StartCoroutine here.
        StartCoroutine(ActionLoop());
    }

    private IEnumerator ActionLoop() {
        List<ActionController> queuedActions = queueController.GetActions();
        List<string> words = new List<string>();
        foreach (var actionController in queuedActions) {
            words.Add(actionController.GetWord());
        }

        for (int i = 0; i < words.Count; i++) {
            playerController.Execute(words[i]);
            queuedActions[i].MakeActive(true);
            foreach (var monster in currentLevelController.monsterControllers) {
                monster.Execute(monster.commands[i % monster.commands.Count]);
            }
            if (queueShouldBeStopped) {
                queuedActions[i].MakeActive(false);
                break;
            }
            yield return new WaitForSeconds(timeBetweenActions);
            queuedActions[i].MakeActive(false);
        }

        yield return new WaitForSeconds(1f);
        
        if (!levelIsWon) {
            ResetLevel();
        }
    }

    private int CalculateActionScore() {
        int amountOfActions = queueController.GetActionCount();
        int maxActions = currentLevelController.maxActionsForMaxPoints;
        int score = maxActionScore - (amountOfActions - maxActions) * actionScoreLoss;
        return Mathf.Max(0, score) + 100;
    }

    private int CalculateTreasureScore() {
        return playerController.GetTreasuresCollected() * treasureValue;
    }

    private int CalculateMonsterScore() {
        return playerController.GetMonstersKilled() * monsterValue;
    }
}
