using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Canvas canvas;

    private CardVisual cardVisual;

    public CardVisual cardVisualPrefab;

    public CardData cardData;

    private bool isFlipped = false;

    private Image cardButton;
    public bool isHovering { get; private set; }

    [System.NonSerialized] public UnityEvent<Card> cardStartFlipEvent = new UnityEvent<Card>();
    [System.NonSerialized] public UnityEvent<Card> cardRevealedEvent = new UnityEvent<Card>();


    
    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        cardButton = GetComponent<Image>();
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


    public void SetFlipped(bool state)
    {
        if (!isFlipped)
        {
            // Flip Card
            isFlipped = state; // Set state immediately when flipping
            cardButton.raycastTarget = false;
            cardStartFlipEvent.Invoke(this);
        }
        
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
        {
            isFlipped = false; // Set state after flip is complete
            cardButton.raycastTarget = true;
        }    
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isFlipped)
            return;

        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}
