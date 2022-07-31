/// <summary>
/// Game Controller
/// Joonas Erho & Melinda Suvivirta, 31.07.2022
/// 
/// This class controls the main gameplay and functions as a communication channel between other classes.
/// </summary>

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
    private readonly float timeBetweenActions = 0.5f;

    // The score that the player will always get when they complete the level.
    private readonly int scoreFromLevelCompetion = 50;

    // The score that a player will get from completing a level with the smallest amount of actions possible.
    private readonly int maxActionScore = 200;

    // The amount of score a player will lose for each action beyound the minimum amount.
    private readonly int actionScoreLoss = 20;

    // Score gained from each collected treasure.
    private readonly int treasureValue = 50;

    // Score gained from each killed monster.
    private readonly int monsterValue = 100;

    // The total amount of score the player has accumulated.
    private int totalPoints = 0;

    // An array of all levels. Set in the Editor. Their order in array also determines their order in game.
    [SerializeField]
    private GameObject[] levels;

    // The index of the current level (loads that level from the levels array).
    [SerializeField]
    private int currentLevelIndex;

    // Prefab for the Action boxes.
    public GameObject actionPrefab;

    // Works as a connection between the objects in a level and this class. Changes as levels change.
    public LevelController currentLevelController;

    public PlayerController playerController;
    public DoorController doorController;

    // These control various UI elements.
    public StoreController storeController;
    public QueueController queueController;
    public VictoryScreenController victoryScreenController;
    public PlayButtonController playButtonController;

    // These render the items while they are in the player's inventory.
    public SpriteRenderer[] inventoryRenderers = new SpriteRenderer[3];
    // This renders the sword if it's picked up.
    public GameObject swordRenderer;
    // The level text in the top-right corner.
    public TextMeshProUGUI levelText;

    // Flags for game events.
    public bool isRunning = false;
    public bool queueShouldBeStopped = false;
    public bool levelIsWon = false;

    void Start() {
        LoadNewLevel(currentLevelIndex);
    }

    /// <summary>
    /// Loads a new level. This is called when the game begins and when the player advances to a new level.
    /// </summary>
    /// <param name="index">Index of new level to be loaded in the levels-array.</param>
    private void LoadNewLevel(int index) {
        ResetQueue();
        LoadLevel(index);
        storeController.GenerateAvailableActions(currentLevelController.actions, actionButtonGap, amountOfActionsInRow, this, actionPrefab);
        ShowLevelText(index);
    }

    /// <summary>
    /// Resets the current level, keeping the created actions in the queue and store areas.
    /// </summary>
    public void ResetLevel() {
        victoryScreenController.gameObject.SetActive(false);
        LoadLevel(currentLevelIndex);
    }

    /// <summary>
    /// Handles the actual loading of a level. Used both in loading a new level and resetting the current one.
    /// </summary>
    /// <param name="index"></param>
    private void LoadLevel(int index) {
        // Reset inventory state. (We don't have to remove the items from the player because this also destroys and creates a new player).
        ToggleSwordVisibility(false);
        foreach (var r in inventoryRenderers) {
            r.sprite = null;
        }

        // Destroy the current level. Only do this if it exists, as otherwise this would throw an error.
        if (currentLevelController != null) {
            Destroy(currentLevelController.gameObject);
            currentLevelController = null;
        }

        // Resets the play button to its original state.
        playButtonController.StopRunning();

        // Generates a new gameobject from the given level and sets it up.
        GameObject go = Instantiate(levels[index]);
        currentLevelController = go.GetComponent<LevelController>();
        currentLevelController.SetConnections(this, out playerController, out doorController);

        // Reset flags.
        queueShouldBeStopped = false;
        levelIsWon = false;
    }

    /// <summary>
    /// Called when the player decides to advance to the next level. Adds the player's score from the current level to the total score
    /// and increments the current level index so the correct level is loaded.
    /// </summary>
    public void GoToNextLevel() {
        // If player chooses to go to next level, add the obtained score to total score.
        totalPoints = totalPoints + CalculateActionScore() + CalculateTreasureScore() + CalculateMonsterScore();
        currentLevelIndex++;
        victoryScreenController.gameObject.SetActive(false);
        LoadNewLevel(currentLevelIndex);
    }

    /// <summary>
    /// Called when the player stands on the door and it has no locks remaining. Stops the execution of the action loop and displays
    /// the victory screen with its appropriate scores.
    /// </summary>
    public void WinLevel() {
        queueShouldBeStopped = true;
        levelIsWon = true;
        victoryScreenController.gameObject.SetActive(true);
        victoryScreenController.DisplayScore(CalculateActionScore(), CalculateTreasureScore(), CalculateMonsterScore(), currentLevelIndex + 1);
        
        // If player won the last level, do not show the buttons and instead show the total score.
        if (currentLevelIndex == levels.Length - 1) {
            totalPoints = totalPoints + CalculateActionScore() + CalculateTreasureScore() + CalculateMonsterScore();
            victoryScreenController.DisplayFinalScore(totalPoints);
        }
    }

    /// <summary>
    /// Updates the level counter in the text on the top-right.
    /// </summary>
    /// <param name="index">Index of the next level.</param>
    public void ShowLevelText(int index) {
        // Increment the index as the number of the level is one greater than it's index in the array.
        levelText.text = (++index).ToString();
    }

    /// <summary>
    /// Toggles whether or not the sword slot of the inventory shows the sword icon, indicating it's
    /// collected.
    /// </summary>
    public void ToggleSwordVisibility(bool b) {
        swordRenderer.SetActive(b);
    }

    /// <summary>
    /// Destroys the monster after a small delay. This is called when the player successfully kills a monster
    /// with the sword.
    /// </summary>
    /// <param name="monster">The monster that should be destroyed.</param>
    public void DestroyMonster(GameObject monster) {
        MonsterController mc = monster.GetComponent<MonsterController>();
        currentLevelController.monsterControllers.Remove(mc);
        Destroy(monster, 0.15f);
    }

    /// <summary>
    /// Transforms the current queue of actions to strings and makes the player execute them.
    /// </summary>
    public void Execute() {
        StartCoroutine(ActionLoop());
    }

    /// <summary>
    /// This action loop is started when the player presses the play button and halted when the player dies,
    /// runs out of commands or wins the game. The player can also manually stop this by pressing the stop
    /// button.
    /// 
    /// This coroutine loops through the commands given to the player and makes it execute them, while also
    /// making any possible monsters with actions in the level do theirs at the same time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActionLoop() {
        isRunning = true;

        // Turn the list of queued actions into a list of strings containing the command names.
        List<ActionController> queuedActions = queueController.GetActions();
        List<string> words = new List<string>();
        foreach (var actionController in queuedActions) {
            words.Add(actionController.GetWord());
        }

        // For each command, make the player execute it. 
        for (int i = 0; i < words.Count; i++) {
            playerController.Execute(words[i]);

            // Highlight the current action.
            queuedActions[i].MakeActive(true);
            
            // Simultaneously execute any actions monsters might have. These loop for as long as the
            // player acts.
            foreach (var monster in currentLevelController.monsterControllers) {
                monster.Execute(monster.commands[i % monster.commands.Count]);
            }

            // If something has enabled this flag, stop this loop and remove the highlight from the
            // current action.
            if (queueShouldBeStopped) {
                queuedActions[i].MakeActive(false);
                break;
            }

            // Otherwise wait for the given time before looping again (and afterwards disable the
            // highlight on the current action).
            yield return new WaitForSeconds(timeBetweenActions);
            queuedActions[i].MakeActive(false);
        }

        // After the loop is finished, the game will wait for a second before doing anything else.
        yield return new WaitForSeconds(1f);
        isRunning = false;

        // If the level is not won, it resets automatically. Otherwise nothing is done, as the player
        // is already looking at the victory screen.
        if (!levelIsWon) {
            ResetLevel();
        }
    }

    /// <summary>
    /// Calculates the amount of score the player should receive from the amount of actions they used
    /// in this level. Adds tje default score the player gets from every level to this.
    /// </summary>
    /// <returns>Score between 250 and 50.</returns>
    private int CalculateActionScore() {
        int amountOfActions = queueController.GetActionCount();
        int maxActions = currentLevelController.maxActionsForMaxPoints;
        int score = maxActionScore - (amountOfActions - maxActions) * actionScoreLoss;
        return Mathf.Clamp(score, 0, maxActionScore) + scoreFromLevelCompetion;
    }

    // Same as above, but for treasures collected.
    private int CalculateTreasureScore() {
        return playerController.GetTreasuresCollected() * treasureValue;
    }

    // ...and monsters killed.
    private int CalculateMonsterScore() {
        return playerController.GetMonstersKilled() * monsterValue;
    }

    public void AddActionToQueue(ActionScriptableObject action) {
        queueController.AddActionToQueue(action, this, actionPrefab);
    }

    public void RemoveActionFromQueue(ActionController ac) {
        queueController.RemoveActionFromQueue(ac);
    }

    public void ResetQueue() {
        queueController.ResetQueue();
    }
}
