using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CardManager : MonoBehaviour
{

    public LayoutCards layoutCards;

    public Card cardPrefab;

    public CardSet cardSet;

    List<CardData> availableCards = new List<CardData>();

    
    public void Awake()
    {
        ResetCardSet();
    }

    public void CreateCards(int layoutSizeX, int layoutSizeY)
    {
        Debug.Log("Create cards " + layoutSizeX + " x " + layoutSizeY);
         
        int cardCount = (layoutSizeX * layoutSizeY) / 2;
        for (int i = 0; i < cardCount; i++)
        { 
            CardData cardData = GetRandomCard();

            for (int x = 0; x < 2; x++)
            {
                Card card = Instantiate(cardPrefab, layoutCards.transform).GetComponent<Card>();
                card.Initialize(cardData);
            }
        }
        layoutCards.itemCountX = layoutSizeX;
        layoutCards.itemCountY = layoutSizeY;
        ShuffleCards();
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

    public void DestroyCards()
    {
        foreach (Transform child in layoutCards.transform)
        {
            Destroy(child.gameObject);
        }
    }

}
