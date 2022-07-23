using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VictoryScreenController : MonoBehaviour
{
    public TextMeshProUGUI actionsUsedScoreText;
    public TextMeshProUGUI treasuresScoreText;
    public TextMeshProUGUI monstersScoreText;
    public TextMeshProUGUI totalScoreText;

    public void DisplayScore(int actionScore, int treasureScore, int monsterScore) {
        actionsUsedScoreText.text = actionScore.ToString();
        treasuresScoreText.text = treasureScore.ToString();
        monstersScoreText.text = monsterScore.ToString();
        totalScoreText.text = (actionScore + treasureScore + monsterScore).ToString();
    }


}
