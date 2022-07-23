using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryScreenController : MonoBehaviour
{
    public TextMeshProUGUI actionsUsedScoreText;
    public TextMeshProUGUI treasuresScoreText;
    public TextMeshProUGUI monstersScoreText;
    public TextMeshProUGUI totalScoreText;

    public GameController gameController;

    public Button tryAgainButton;
    public Button nextLevelButton;

    void Awake() {
        tryAgainButton.onClick.AddListener(gameController.ResetLevel);
        nextLevelButton.onClick.AddListener(gameController.GoToNextLevel);
    }

    public void DisplayScore(int actionScore, int treasureScore, int monsterScore) {
        actionsUsedScoreText.text = actionScore.ToString();
        treasuresScoreText.text = treasureScore.ToString();
        monstersScoreText.text = monsterScore.ToString();
        totalScoreText.text = (actionScore + treasureScore + monsterScore).ToString();
    }
}
