using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public int[,] grid;
    Vector2Int gridSize = new Vector2Int(5, 5);
    void Start()
    {
        RandomizeGrid();
    }

    private void Init()
    {
        grid = new int[gridSize.x, gridSize.y];
    }

    private void RandomizeGrid()
    {

        for (int y = gridSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                grid[x, y] = Random.Range(0, 9);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            PrintGrid();
    }
    void PrintGrid()
    {
        string txt = string.Empty;

        for (int y = gridSize.y - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                txt += grid[x, y].ToString();
            }
        }
    }

}
