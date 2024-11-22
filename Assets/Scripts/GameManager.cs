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

    Coroutine onGameWinCoroutine;

    void Start()
    {
        levelSelectionManager.LoadPreviousGameState();
        cardManager.onGameWinEvent.AddListener(OnGameWin);
    }

    public void StartNewGame(LevelDefinition levelDef)
    {

        if (onGameWinCoroutine != null)
            StopCoroutine(onGameWinCoroutine);

        // Init Game Config / (To Do) Restore Save Game 
        playerScore.Reset();

        // Init UI
        levelComplete.SetActive(false);

        cardManager.CreateCards(levelDef);

        // Start Game

    }

    public void OnGameWin()
    {
        onGameWinCoroutine = StartCoroutine(OnGameWinSequence());
        RemoveAllSaveData();
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

    public void RemoveAllSaveData()
    {
        cardManager.RemoveSaveState();
    }
}
