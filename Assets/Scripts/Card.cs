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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isFlipped)
            return;
        
        isFlipped = true;
        cardVisual.SetIsFlipped(true, true);


    }


}
