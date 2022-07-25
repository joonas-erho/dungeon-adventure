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

    public void DisplayScore(int actionScore, int treasureScore, int monsterScore, int currentLevel) {
        congratulationText.text = randomCongratulations[Random.Range(0,randomCongratulations.Length)];
        levelText.text = "Level " + currentLevel + " completed!";
        actionsUsedScoreText.text = actionScore.ToString();
        treasuresScoreText.text = treasureScore.ToString();
        monstersScoreText.text = monsterScore.ToString();
        totalScoreText.text = (actionScore + treasureScore + monsterScore).ToString();
    }
}
