using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Canvas canvas;

    private CardVisual cardVisual;

    public CardVisual cardVisualPrefab;

    public CardData cardData;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void Initialize(CardData _cardData)
    {
        cardData = _cardData;
        gameObject.name += " " + cardData.name;
    }

    void Start()
    {
        cardVisual = Instantiate(cardVisualPrefab, transform).GetComponent<CardVisual>();

        cardVisual.Initialize(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
