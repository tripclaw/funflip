using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler
{
    private Canvas canvas;

    private CardVisual cardVisual;

    public CardVisual cardVisualPrefab;

    public CardData cardData;

    private bool isFlipped = false;

    [System.NonSerialized] public UnityEvent<Card> cardRevealedEvent = new UnityEvent<Card>();

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
        cardVisual.cardFlipCompleteEvent.AddListener(OnCardFlipComplete);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFlipped(bool state)
    {
        if (!isFlipped)
            isFlipped = state; // Set state immediately when flipping
        
        cardVisual.SetIsFlipped(state, true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isFlipped)
            return;

        SetFlipped(true);
    }


    void OnCardFlipComplete(bool state)
    {

        if (state)
            cardRevealedEvent.Invoke(this);
        else
            isFlipped = false; // Set state after flip is complete
    }


}
