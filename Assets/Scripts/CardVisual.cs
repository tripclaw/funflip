using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{

    [SerializeField] Image itemImage;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image faceBackImage;
    [SerializeField] Image faceFrontImage;

    private bool isFlipped = false;
    private Card parentCard;
    private Transform cardTransform;

    public void Initialize(Card _parentCard)
    {
        parentCard = _parentCard;
        cardTransform = parentCard.transform;

        gameObject.name = _parentCard.name + " - " + gameObject.name;

        RectTransform rectTransform = transform as RectTransform;
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = _parentCard.transform.GetComponent<RectTransform>().rect.size;

        itemImage.sprite = _parentCard.cardData.cardFaceSprite;
        backgroundImage.color = _parentCard.cardData.bgColor;
    
    
    }

    void Start()
    {
        
    }

    void SetIsFlipped(bool state)
    {

        isFlipped = state;

        if (isFlipped)
            faceBackImage.transform.SetAsFirstSibling();
        else
            faceBackImage.transform.SetAsLastSibling();
    }


}
