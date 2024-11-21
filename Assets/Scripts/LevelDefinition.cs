using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelDefinition
{
    public string name;
    public Vector2Int size = new Vector2Int(3, 4);
    public int[] emptyCardIndexes;

    public LevelDefinition(int sizeX, int sizeY)
    {
        name = sizeX + "x" + sizeY;
        size = new Vector2Int(sizeX, sizeY);        
    }
}