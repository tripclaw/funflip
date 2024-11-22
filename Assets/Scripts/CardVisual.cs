using System.Collections;
using System.Collections.Generic;
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
    private RectTransform rectTransform;

    [System.NonSerialized] public UnityEvent<bool> cardFlipCompleteEvent = new UnityEvent<bool>();


    [Header("Flip Animation")]
    [SerializeField] float flipDuration = 0.45f;
    [SerializeField] AnimationCurve flipAnimCurve;
    [SerializeField] float nonMatchDelay = 0.5f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

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

        SetIsFlipped(_parentCard.isFlipped);


    }

    float targetHoverHeight = 0f;
    float currentHoverHeight = 0f;
    float hoverSpeed = 10f;

    private void Update()
    {
        if (isAnimating)
        {
            if (transform.eulerAngles.y > 90)
                SetSpriteOrder(true);
            else
                SetSpriteOrder(false);
        }

        if (parentCard.isHovering)
        {
            targetHoverHeight = 12f;
        }
        else
        {
            targetHoverHeight = 0f;
        }

        currentHoverHeight = Mathf.Lerp(currentHoverHeight, targetHoverHeight, hoverSpeed * Time.deltaTime);
        rectTransform.anchoredPosition = new Vector2(currentHoverHeight/2f, currentHoverHeight);
        // shadow.rectTransform.anchoredPosition = new Vector2(-currentHoverHeight * 2f, -currentHoverHeight * 2f);

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
        float duration = isFlipped ? flipDuration : flipDuration * 0.66f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float rot = Mathf.Lerp(startRotation, endRotation, flipAnimCurve.Evaluate(t / duration));

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rot, transform.eulerAngles.z);

            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, endRotation, transform.eulerAngles.z);

        if (isFlipped)
            yield return new WaitForSeconds(nonMatchDelay);

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
