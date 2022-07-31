/// <summary>
/// Victory Screen Controller
/// Joonas Erho & Melinda Suvivirta, 31.07.2022
/// 
/// This class controls the victory screen, shown when the player beats a level.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryScreenController : MonoBehaviour
{
    public TextMeshProUGUI congratulationText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI actionsUsedScoreText;
    public TextMeshProUGUI treasuresScoreText;
    public TextMeshProUGUI monstersScoreText;
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI finalScoreText;

    public GameController gameController;

    public Button tryAgainButton;
    public Button nextLevelButton;

    private string[] randomCongratulations = new string[] {
        "Nice Job!",
        "Well Done!",
        "Great Work!",
        "Excellent Job!",
        "You Got It!",
        "That Was Easy"
    };

    void Awake() {
        tryAgainButton.onClick.AddListener(gameController.ResetLevel);
        nextLevelButton.onClick.AddListener(gameController.GoToNextLevel);
    }

    /// <summary>
    /// Updates the texts in the victory screen.
    /// </summary>
    /// <param name="actionScore">Score from amount of actions.</param>
    /// <param name="treasureScore">Score from treasure collected.</param>
    /// <param name="monsterScore">Score from monsters killed.</param>
    /// <param name="currentLevel">Number of current level.</param>
    public void DisplayScore(int actionScore, int treasureScore, int monsterScore, int currentLevel) {
        congratulationText.text = randomCongratulations[Random.Range(0,randomCongratulations.Length)];
        levelText.text = "Level " + currentLevel + " completed!";
        actionsUsedScoreText.text = actionScore.ToString();
        treasuresScoreText.text = treasureScore.ToString();
        monstersScoreText.text = monsterScore.ToString();
        totalScoreText.text = (actionScore + treasureScore + monsterScore).ToString();
    }

    /// <summary>
    /// Disables the "try again"- and "next level"-buttons and displays the player's final score.
    /// </summary>
    /// <param name="totalScore"></param>
    public void DisplayFinalScore(int totalScore) {
        tryAgainButton.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(true);
        finalScoreText.text = "Your total score:\n" + totalScore;
    }
}
