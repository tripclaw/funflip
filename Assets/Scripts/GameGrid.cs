using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GameGrid
{
    private int[,] grid;

    void Start()
    {
        RandomizeGrid(32);
    }

    public GameGrid(int sizeX, int sizeY)
    {
        this.grid = new int[sizeX, sizeY];
    }


    private void RandomizeGrid(int numberOfCards)
    {

        int sizeX = grid.GetLength(0);
        int sizeY = grid.GetLength(1);

        for (int y = sizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < sizeX; x++)
            {
                grid[x, y] = UnityEngine.Random.Range(0, 9);
            }
        }
    }

    public int[,] GetGameGrid()
    {
        return grid;
    }

}
