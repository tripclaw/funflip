using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{

    public LayoutCards layoutCards;
    public PlayerScore playerScore;

    public Card cardPrefab;

    public CardSet cardSet;

    List<Card> currentCards = new List<Card>();
    List<Card> selectedCards = new List<Card>();

    List<int> availableCardIndexes = new List<int>();
    List<int> chosenCardIndexes = new List<int>(); 

    int matchesNeeded = 1;
    int totalMatchesMade = 0;
    
    int comboLevel = 0;

    [HideInInspector]
    public UnityEvent onGameWinEvent = new UnityEvent();

    [Header("Sounds")]
    [SerializeField] AudioClip[] cardFlipSounds;
    [SerializeField] AudioClip[] cardMatchSounds;
    [SerializeField] AudioClip[] cardMismatchSounds;

    public void Awake()
    {

    }

    #region Card Creation
    public void CreateCards(LevelDefinition levelDef)
    {

        if (currentCards.Count > 0)
            DestroyCards();

        bool offByOne = false;

        offByOne = (levelDef.size.x * levelDef.size.y) % 2 == 1;
        int emptyCount = levelDef.emptyCardIndexes.Count();
        int size = (levelDef.size.x * levelDef.size.y) - emptyCount;
        int cardPairsCount = size / 2;

        cardPairsCount -= (offByOne ? emptyCount - 1 : emptyCount);

        matchesNeeded = cardPairsCount; 

        Debug.Log("Creating card layout " + levelDef.size.x + " x " + levelDef.size.y + " card pairs count: " + cardPairsCount);

        ResetAvailableCardIndexes();      

        bool loadedState = LoadCardsShuffleState();

        if (!loadedState)
        {
            chosenCardIndexes = GetRandomCardsFromCardSet(cardPairsCount);
            Debug.Log("Shuffling Cards...");
            ShuffleCardIndexes();
            SaveCardsShuffleState();
        }

        for (int i = 0; i < chosenCardIndexes.Count; i++)
        {
            // Make cards from chosen card indexes
            CardData cardDataCopy = Instantiate(cardSet.cards[chosenCardIndexes[i]]);           
            Card card = Instantiate(cardPrefab, layoutCards.transform).GetComponent<Card>();
            card.Initialize(cardDataCopy);
            currentCards.Add(card);
            card.cardRevealedEvent.AddListener(OnCardFlipped);
            card.cardStartFlipEvent.AddListener(OnCardStartFlip);
        }

        layoutCards.SetDimensions(levelDef.size.x, levelDef.size.y);

        StartCoroutine(AddEmptyCardsLate(levelDef));

        totalMatchesMade = 0;
        if (loadedState)
        {
            LoadCardsMatchedState();
            LoadComboLevelState();
        }
    }

    public void AddEmptyCards(LevelDefinition levelDef)
    {
        foreach (int emptyCardIndex in levelDef.emptyCardIndexes)
        {
            GameObject emptyPlaceholder = new GameObject();
            emptyPlaceholder.AddComponent<RectTransform>();
            emptyPlaceholder.name = "Empty";
            emptyPlaceholder.transform.SetParent(layoutCards.transform);
            emptyPlaceholder.transform.SetSiblingIndex(emptyCardIndex);
            for (int i = emptyCardIndex + 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).SetSiblingIndex(transform.GetChild(i).GetSiblingIndex() + 1);
            }
            // Debug.Log("creating empty index " + emptyCardIndex + " actual: " + emptyPlaceholder.transform.GetSiblingIndex());
        }

    }

    IEnumerator AddEmptyCardsLate(LevelDefinition levelDef)
    {
        yield return new WaitForEndOfFrame();
        AddEmptyCards(levelDef);
        layoutCards.UpdateGrid();
    }

    public void DestroyCards()
    {
        selectedCards.Clear();

        foreach (Card card in currentCards)
        {
            card.cardRevealedEvent.RemoveListener(OnCardFlipped);
        }
        foreach (Transform child in layoutCards.transform)
        {
            Destroy(child.gameObject);
        }

        currentCards.Clear();
    }

    void ShuffleCardIndexes()
    {
        List<int> allIndexes = new List<int>();
        for (int i = 0; i < chosenCardIndexes.Count; i++)
            allIndexes.Add(i);

        int count = chosenCardIndexes.Count;
        for (var i = 0; i < count - 1; ++i)
        {
            int rand = UnityEngine.Random.Range(i, count);
            int temp = chosenCardIndexes[i];
            chosenCardIndexes[i] = chosenCardIndexes[rand];
            chosenCardIndexes[rand] = temp;
        }
    }

    List<int> GetRandomCardsFromCardSet(int cardPairsCount)
    {
        List<int> cardIndexes = new List<int>();

        for (int i = 0; i < cardPairsCount; i++)
        {
            int cardIndex = GetRandomCardIndex();

            // Add a pair of each random card
            for (int x = 0; x < 2; x++)
            {
                cardIndexes.Add(cardIndex);
            }            
        }

        return cardIndexes;
    }

    void ResetAvailableCardIndexes()
    {
        availableCardIndexes.Clear();
        for (int i = 0; i < cardSet.cards.Length; i++)
        {
            availableCardIndexes.Add(i);
        }
    }

    int GetRandomCardIndex()
    {
        int i = UnityEngine.Random.Range(0, availableCardIndexes.Count);
        int chosenCardIndex = availableCardIndexes[i];
        availableCardIndexes.RemoveAt(i);
        return chosenCardIndex;
    }
    #endregion

    #region Events from Cards
    void OnCardStartFlip(Card card)
    {
        PlayCardFlipSound();
    }
    
    void OnCardFlipped(Card card)
    {
        selectedCards.Add(card);
        if (selectedCards.Count == 2)
        {
            CheckCardsForMatch(selectedCards[0], selectedCards[1]);
                
            selectedCards.Clear();
        }
    }
    #endregion

    #region Card Game Logic
    void CheckCardsForMatch(Card card1, Card card2)
    {
        if (card1.cardData.name == card2.cardData.name)
        {
            // Match!
            comboLevel++;
            playerScore.AddScore(100 * comboLevel, comboLevel);
            totalMatchesMade++;
            PlayMatchSound();
            card1.SetMatched(true);
            card2.SetMatched(true);

            if (totalMatchesMade >= matchesNeeded)
            {
                // Win State
                GameWin();
            }
            else
            {
                SaveComboLevelState();
                SaveCardsMatchedState();
                playerScore.SaveScoreState(true);
            }
        }
        else
        {
            // Not a match
            card1.SetFlipped(false);
            card2.SetFlipped(false);
            comboLevel = 0;
            playerScore.SetComboLevel(0);
            SaveComboLevelState(true);
            PlayMismatchSound();
        }
        // Debug.Log("totalMatchesMade: " + totalMatchesMade + " matchesNeeded: " + matchesNeeded);
    }

    void GameWin()
    {
        onGameWinEvent.Invoke();
    }
    #endregion

    #region Sound
    void PlayCardFlipSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, cardFlipSounds.Length);
        AudioSource.PlayClipAtPoint(cardFlipSounds[randomIndex], transform.position);
    }
    void PlayMatchSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, cardMatchSounds.Length);
        AudioSource.PlayClipAtPoint(cardMatchSounds[randomIndex], transform.position, 0.8f);
    
    }
    void PlayMismatchSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, cardMismatchSounds.Length);
        AudioSource.PlayClipAtPoint(cardMismatchSounds[randomIndex], transform.position);
    }
    #endregion

    #region Save and Load
    public void SaveCardsShuffleState(bool saveNow = false)
    {
        // Save randomized card order
        string chosenCardIndexesTxt = JsonUtility.ToJson(new JsonListWrapper<int>(chosenCardIndexes));
       
        Debug.Log("SaveCardsShuffleState: " + chosenCardIndexesTxt);       
               
        PlayerPrefs.SetString("save_chosenCardIndexes", chosenCardIndexesTxt);

        if (saveNow) PlayerPrefs.Save();
    }

    bool LoadCardsShuffleState()
    {
        if (!PlayerPrefs.HasKey("save_chosenCardIndexes"))
            return false;

        string chosenCardIndexesTxt = PlayerPrefs.GetString("save_chosenCardIndexes", string.Empty);
        chosenCardIndexes = JsonUtility.FromJson<JsonListWrapper<int>>(chosenCardIndexesTxt).list;
        int cardsNeeded = matchesNeeded * 2;
        if (chosenCardIndexes.Count != cardsNeeded)
        {
            Debug.LogWarning("Loaded state does not contain correct length, aborting. chosenCardIndexes.Count: "
                                + chosenCardIndexes.Count + " cardsNeeded: " + cardsNeeded);
            return false;
        }
        Debug.Log("Loaded Card Shuffle State: " + chosenCardIndexesTxt);

        return true;
    }


    public void SaveCardsMatchedState(bool saveNow = false)
    {
        List<bool> cardsMatched = new List<bool>();
        for (int i = 0; i < currentCards.Count; i++)
        {
            cardsMatched.Add(currentCards[i].isMatched);
        }

        string cardsMatchedTxt = JsonUtility.ToJson(new JsonListWrapper<bool>(cardsMatched));
        Debug.Log("SaveCardsMatchedState:" + cardsMatchedTxt);

        PlayerPrefs.SetString("save_cardsMatched", cardsMatchedTxt);

        if (saveNow) PlayerPrefs.Save();
    }


    bool LoadCardsMatchedState()
    {
        if (!PlayerPrefs.HasKey("save_cardsMatched"))
            return false;

        string cardsMatchedTxt = PlayerPrefs.GetString("save_cardsMatched", string.Empty);
        List<bool> cardsMatched = JsonUtility.FromJson<JsonListWrapper<bool>>(cardsMatchedTxt).list;

        if (cardsMatched.Count != currentCards.Count)
        {
            Debug.LogWarning("Loaded state does not contain correct length, aborting. cardsMatched.Count: "
                                + cardsMatched.Count + " should be : " + currentCards.Count);
            return false;
        }

        for (int i = 0; i < currentCards.Count; i++)
        {
            if (cardsMatched[i] == true)
            {
                currentCards[i].SetMatched(true, true);
                totalMatchesMade++;
            }
        }
        totalMatchesMade /= 2;
        Debug.Log("Loaded Card Matched State: " + cardsMatchedTxt + " totalMatchesMade: " + totalMatchesMade);

        return true;
    }

    void SaveComboLevelState(bool saveNow = false)
    {
        PlayerPrefs.SetInt("save_comboLevel", comboLevel);
        //Debug.Log("save combo level:" + comboLevel);
        if (saveNow) PlayerPrefs.Save();
    }

    void LoadComboLevelState()
    {
        comboLevel = PlayerPrefs.GetInt("save_comboLevel", 0);
        // Debug.Log("load combo level:" + comboLevel);
    }

    public void RemoveSaveState()
    {
        PlayerPrefs.DeleteKey("save_chosenCardIndexes");
        PlayerPrefs.DeleteKey("save_cardsMatched");
        PlayerPrefs.DeleteKey("save_comboLevel");

        PlayerPrefs.Save();
    }

    #endregion


}
