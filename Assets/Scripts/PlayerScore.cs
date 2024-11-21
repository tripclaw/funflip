using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScore : MonoBehaviour {

    public int score { get; private set; }
    public int lastAddedScore { get; private set; }

    [SerializeField] PlayerScoreUI playerScoreUI;


    public void Awake()
    {
        playerScoreUI.Init(this);
    }

    public void Reset()
    {
        score = 0;
        lastAddedScore = 0;
        playerScoreUI.Reset();
        UpdateUI(false);
    }

    public void AddScore(int points)
    {
        score += points;
        lastAddedScore = points;
        UpdateUI(true);
    }

    void UpdateUI(bool animate)
    {
        playerScoreUI.UpdatePlayerScore(animate);
    }

}
