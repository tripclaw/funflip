using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScore : MonoBehaviour {

    public int score { get; private set; }
    public int lastAddedScore { get; private set; }
    public int lastComboLevel { get; private set; }

    [SerializeField] PlayerScoreUI playerScoreUI;

    public bool isCounting { get { return playerScoreUI.isCounting; } }
    public void Awake()
    {
        playerScoreUI.Init(this);
    }

    public void Reset()
    {
        score = 0;
        lastComboLevel = 0;
        lastAddedScore = 0;
        playerScoreUI.Reset();
        UpdateUI(false);
    }

    public void AddScore(int points, int comboLevel)
    {
        score += points;
        lastComboLevel = comboLevel;
        lastAddedScore = points;
        UpdateUI(true);
    }

    public void SetComboLevel(int level)
    {
        lastComboLevel = level;
    }

    void UpdateUI(bool animate)
    {
        playerScoreUI.UpdatePlayerScore(animate);
    }

}
