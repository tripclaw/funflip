using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Set", menuName = "ScriptableObjects/CardSet", order = 1)]
public class CardSet : ScriptableObject
{

    public CardData[] cards;

}