using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{

    [SerializeField]
    GameManager gameManager;

    [System.Serializable]
    public class LevelDefinition
    {
        public string name;
        public Vector2Int size = new Vector2Int(3, 4);
    }

    [SerializeField]
    LevelDefinition[] levels;

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
        PlayerPrefs.SetInt("CurrentLevel", -1);
        PlayerPrefs.Save();
    }

    public void StartLevel(int level)
    {
        if (level < 0 || level - 1 > levels.Length)
        {
            Debug.LogError("Level out of range: " + level + " / " + levels.Length);
            level = 0;
        }
        
        gameManager.StartLevel(levels[level].size.x, levels[level].size.y);
        currentLevel = level;
        PlayerPrefs.SetInt("CurrentLevel", level);
        PlayerPrefs.Save();
        

        levelSelectionPanel.SetActive(false);

    }

}
