using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{

    public Sprite cardFaceSprite;

    public Color bgColor = new Color(0.4628621f, 0.7661347f, 0.9773585f);



}