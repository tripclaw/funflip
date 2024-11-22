using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    LevelData levelData;

    [SerializeField]
    GameObject levelSelectionPanel;

    private int currentLevel = 0;

    public void LoadPreviousGameState()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", -1);
        if (currentLevel == -1)
        {
            // No previous game state, show level selection
            currentLevel = 0;
            ShowLevelSelectionPanel();
        }
        else
        {
            // Resume previous level
            StartLevel(currentLevel);
        }
               
    }

    public void ShowLevelSelectionPanel()
    {
        levelSelectionPanel.SetActive(true);
        gameManager.RemoveAllSaveData();
        PlayerPrefs.SetInt("CurrentLevel", -1);
        PlayerPrefs.Save();
    }

    public void StartLevel(int level)
    {
        if (level < 0 || level - 1 > levelData.levels.Length)
        {
            Debug.LogError("Level out of range: " + level + " / " + levelData.levels.Length);
            level = 0;
        }
        
        gameManager.StartNewGame(levelData.levels[level]);
        currentLevel = level;
        PlayerPrefs.SetInt("CurrentLevel", level);
        PlayerPrefs.Save();
        

        levelSelectionPanel.SetActive(false);

    }

}
