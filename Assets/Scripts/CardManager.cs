using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public LayoutCards layoutCards;

    public void CreateCards(int layoutSizeX, int layoutSizeY)
    {
        
    }

    public void DestroyCards()
    {
        foreach (Transform child in layoutCards.transform)
        {
            Destroy(child.gameObject);
        }
    }

}
