using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CardVisual : MonoBehaviour
{

    [SerializeField] Image itemImage;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image faceBackImage;
    [SerializeField] Image faceFrontImage;

    private bool isFlipped = false;
    private bool isAnimating = false;

    private Card parentCard;
    private Transform cardTransform;

    [System.NonSerialized] public UnityEvent<bool> cardFlipCompleteEvent = new UnityEvent<bool>();


    [Header("Flip Animation")]
    [SerializeField] float flipDuration = 0.7f;
    [SerializeField] AnimationCurve flipAnimCurve;

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

        SetIsFlipped(false);
    }

    void Start()
    {
        
    }

    private void Update()
    {
        if (isAnimating)
        {
            if (transform.eulerAngles.y > 90)
                SetSpriteOrder(true);
            else
                SetSpriteOrder(false);
        }
    }


    public void SetIsFlipped(bool state, bool animate = false)
    {

        isFlipped = state;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, isFlipped ? 180f : 0f, transform.eulerAngles.z);

        if (animate)
        { 
            StartCoroutine(AnimateFlip());
        }
        else
        {
            SetSpriteOrder(isFlipped);
        }
    
    }

    IEnumerator AnimateFlip()
    {
        isAnimating = true;

        float startRotation = isFlipped ? 0f : 180f;
        float endRotation = isFlipped ? 180f : 0f;
        float t = 0.0f;
        

        while (t < flipDuration)
        {
            t += Time.deltaTime;

            float rot = Mathf.Lerp(startRotation, endRotation, flipAnimCurve.Evaluate(t / flipDuration));

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rot, transform.eulerAngles.z);

            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, endRotation, transform.eulerAngles.z);

        isAnimating = false;

        cardFlipCompleteEvent.Invoke(isFlipped);

    }

    void SetSpriteOrder(bool state)
    {
        if (state)
            faceBackImage.transform.SetAsFirstSibling();
        else
            faceBackImage.transform.SetAsLastSibling();
    }


}
