using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardManager cardManager;

    [SerializeField] LevelSelectionManager levelSelectionManager;

    [SerializeField] PlayerScore playerScore;

    // Start is called before the first frame update
    void Start()
    {
        levelSelectionManager.LoadPreviousGameState();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
            StartLevel(2, 3);
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            StartLevel(3, 4);
        else if (Input.GetKeyUp(KeyCode.Alpha3))
            StartLevel(4, 4);
        else if (Input.GetKeyUp(KeyCode.Alpha4))
            StartLevel(6, 4);
        else if (Input.GetKeyUp(KeyCode.Alpha5))
            StartLevel(5, 6);
        else if (Input.GetKeyUp(KeyCode.Alpha6))
            StartLevel(6, 6);
        else if (Input.GetKeyUp(KeyCode.Alpha7))
            StartLevel(8, 8);

    }

    void CreateGame(int sizeX, int sizeY)
    {
        // Init Game Config / (To Do) Restore Save Game 
        playerScore.Reset();

        // Init UI
        cardManager.CreateCards(sizeX, sizeY);

        // Start Game

    }

    public void StartLevel(int sizeX, int sizeY)
    {
        CreateGame(sizeX, sizeY);
    }

}
