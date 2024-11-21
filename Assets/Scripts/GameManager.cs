using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] CardManager cardManager;
    
    // Start is called before the first frame update
    void Start()
    {
        // Init Player

        // Init Game Config / Restore Save Game

        // Init UI
        cardManager.CreateCards(3, 4);

        // Start Game
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
