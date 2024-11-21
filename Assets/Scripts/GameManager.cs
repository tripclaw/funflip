using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardManager cardManager;

    [SerializeField] LevelSelectionManager levelSelectionManager;

    [SerializeField] PlayerScore playerScore;

    [SerializeField] GameObject levelComplete;

    [Header("Sounds")]
    [SerializeField] AudioClip gameWinSound;

    // Start is called before the first frame update
    void Start()
    {
        levelSelectionManager.LoadPreviousGameState();
        cardManager.onGameWinEvent.AddListener(OnGameWin);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
            StartNewGame(2, 3);
        else if (Input.GetKeyUp(KeyCode.Alpha2))
            StartNewGame(3, 4);
        else if (Input.GetKeyUp(KeyCode.Alpha3))
            StartNewGame(4, 4);
        else if (Input.GetKeyUp(KeyCode.Alpha4))
            StartNewGame(6, 4);
        else if (Input.GetKeyUp(KeyCode.Alpha5))
            StartNewGame(5, 6);
        else if (Input.GetKeyUp(KeyCode.Alpha6))
            StartNewGame(6, 6);
        else if (Input.GetKeyUp(KeyCode.Alpha7))
            StartNewGame(8, 8);

    }

    public void StartNewGame(int sizeX, int sizeY)
    {

        if (onGameWinCoroutine != null)
            StopCoroutine(onGameWinCoroutine);

        // Init Game Config / (To Do) Restore Save Game 
        playerScore.Reset();

        // Init UI
        levelComplete.SetActive(false);

        cardManager.CreateCards(sizeX, sizeY);

        // Start Game

    }

    public void StartLevel(int sizeX, int sizeY)
    {
        StartNewGame(sizeX, sizeY);
    }

    Coroutine onGameWinCoroutine;
    public void OnGameWin()
    {
        onGameWinCoroutine = StartCoroutine(OnGameWinSequence());       
    }

    IEnumerator OnGameWinSequence()
    {
        while (playerScore.isCounting)
            yield return null;

        PlayWinSound();

        levelComplete.SetActive(true);

    }

    void PlayWinSound()
    {
        AudioSource.PlayClipAtPoint(gameWinSound, transform.position);
    }
}
