using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CardManager : MonoBehaviour
{

    public LayoutCards layoutCards;
    public PlayerScore playerScore;

    public Card cardPrefab;

    public CardSet cardSet;

    List<CardData> availableCards = new List<CardData>();
    List<Card> currentCards = new List<Card>();
    List<Card> selectedCards = new List<Card>();

    public void Awake()
    {

    }

    public void CreateCards(int layoutSizeX, int layoutSizeY)
    {
        Debug.Log("Creating card layout " + layoutSizeX + " x " + layoutSizeY);

        if (currentCards.Count > 0)
            DestroyCards();

        ResetCardSet();

        int cardCount = (layoutSizeX * layoutSizeY) / 2;
        for (int i = 0; i < cardCount; i++)
        { 
            CardData cardData = GetRandomCard();

            for (int x = 0; x < 2; x++)
            {
                Card card = Instantiate(cardPrefab, layoutCards.transform).GetComponent<Card>();
                card.Initialize(cardData);
                currentCards.Add(card);
                card.cardRevealedEvent.AddListener(OnCardFlipped);
            }
        }
        layoutCards.SetDimensions(layoutSizeX, layoutSizeY);
        ShuffleCards();
    }
    public void DestroyCards()
    {
        selectedCards.Clear();

        foreach (Card card in currentCards)
        {
            card.cardRevealedEvent.RemoveListener(OnCardFlipped);
            Destroy(card.gameObject);
        }
        currentCards.Clear();
    }

    void ShuffleCards()
    {
        for (int i = 0; i < layoutCards.transform.childCount; i++)
        {
            layoutCards.transform.GetChild(i).SetSiblingIndex(UnityEngine.Random.Range(0, layoutCards.transform.childCount));
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
            playerScore.AddScore(100);
        }
        else
        {
            // Not a match
            card1.SetFlipped(false);
            card2.SetFlipped(false);
        }

    }
}
