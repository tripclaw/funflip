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

    List<CardData> availableCards = new List<CardData>();
    List<Card> currentCards = new List<Card>();
    List<Card> selectedCards = new List<Card>();

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

    public void CreateCards(LevelDefinition levelDef)
    {

        
        Debug.Log("Creating card layout " + levelDef.size.x + " x " + levelDef.size.y);

        if (currentCards.Count > 0)
            DestroyCards();

        ResetCardSet();
        matchesNeeded = 0;
        bool offByOne = false;

        offByOne = (levelDef.size.x * levelDef.size.y) % 2 == 1;
        int emptyCount = levelDef.emptyCardIndexes.Count();
        int size = (levelDef.size.x * levelDef.size.y) - emptyCount;
        int cardCount = size / 2;

        cardCount -= (offByOne ? emptyCount - 1 : emptyCount);

        List<CardData> cardDataList = new List<CardData>();

        for (int i = 0; i < cardCount; i++)
        {
            CardData cardData = GetRandomCard();

            // Add a pair of each random card
            for (int x = 0; x < 2; x++)
            {
                cardDataList.Add(cardData);
            }
            matchesNeeded++;
        }

        // shuffle cardDataList
        ShuffleCardDataList(cardDataList);

        foreach (CardData cd in cardDataList)
        {                
            Card card = Instantiate(cardPrefab, layoutCards.transform).GetComponent<Card>();
            card.Initialize(cd);
            currentCards.Add(card);
            card.cardRevealedEvent.AddListener(OnCardFlipped);
            card.cardStartFlipEvent.AddListener(OnCardStartFlip);
        }

        layoutCards.SetDimensions(levelDef.size.x, levelDef.size.y);

        // ShuffleCardsBySiblingIndex();

        StartCoroutine(AddEmptyCardsLate(levelDef));      

        totalMatchesMade = 0;
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

    void ShuffleCardsBySiblingIndex()
    {
        List<int> allIndexes = new List<int>();
        for (int i = 0; i < layoutCards.transform.childCount; i++)
            allIndexes.Add(i);

        for (int i = 0; i < layoutCards.transform.childCount; i++)
        {
            int pickedIndex = UnityEngine.Random.Range(0, allIndexes.Count);
            int randomSiblingIndex = allIndexes[pickedIndex];
            allIndexes.RemoveAt(pickedIndex);
            layoutCards.transform.GetChild(i).SetSiblingIndex(randomSiblingIndex);
        }
    }

    void ShuffleCardDataList(List<CardData>cardDataList)
    {
        List<int> allIndexes = new List<int>();
        for (int i = 0; i < cardDataList.Count; i++)
            allIndexes.Add(i);

        int count = cardDataList.Count;
        for (var i = 0; i < count - 1; ++i)
        {
            int rand = UnityEngine.Random.Range(i, count);
            CardData tempCard = cardDataList[i];
            cardDataList[i] = cardDataList[rand];
            cardDataList[rand] = tempCard;
        }
    }

    void ResetCardSet()
    {
        availableCards.Clear();
        for (int i = 0; i < cardSet.cards.Length; i++)
        {
            availableCards.Add(cardSet.cards[i]);
        }
    }

    CardData GetRandomCard()
    {
        int i = UnityEngine.Random.Range(0, availableCards.Count);
        CardData cardDataCopy = Instantiate(availableCards[i]);
        availableCards.RemoveAt(i);
        return cardDataCopy;
    }

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

    void CheckCardsForMatch(Card card1, Card card2)
    {
        if (card1.cardData.name == card2.cardData.name)
        {
            // Match!
            comboLevel++;
            playerScore.AddScore(100 * comboLevel, comboLevel);
            totalMatchesMade++;
            PlayMatchSound();

            if (totalMatchesMade >= matchesNeeded)
            {
                // Win State
                GameWin();
            }
        }
        else
        {
            // Not a match
            card1.SetFlipped(false);
            card2.SetFlipped(false);
            comboLevel = 0;
            playerScore.SetComboLevel(0);
            PlayMismatchSound();
        }

    }

    void GameWin()
    {
        onGameWinEvent.Invoke();
    }

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

}
