using System.Collections;
using System.Collections.Generic;
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

    #region Save and Load
    public void SaveScoreState(bool saveNow = false)
    {
        PlayerPrefs.SetInt("save_score", score);

        if (saveNow) PlayerPrefs.Save();
    }

    public void LoadScoreState()
    {
        if (PlayerPrefs.HasKey("save_score"))
        {
            score = PlayerPrefs.GetInt("save_score", 0);
            playerScoreUI.Reset();
            UpdateUI(false);
        }
        else
        {
            Reset();
        }
    }

    public void RemoveSaveState()
    {
        PlayerPrefs.DeleteKey("save_score");

        PlayerPrefs.Save();
    }

    #endregion

}
