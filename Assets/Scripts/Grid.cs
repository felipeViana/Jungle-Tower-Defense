using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        this.gridArray = new int[width, height];
        this.debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugTextArray[x, y] = Utils.CreateWorldText(gridArray[x, y].ToString(), Color.white, GetWorldPosition(x, y) + new Vector3(cellSize / 2, cellSize / 2));

                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public void SetValue(int x, int y, int value)
    {
        if (isValid(x, y))
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = value.ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        Vector2 coordinates = GetXY(worldPosition);
        SetValue((int)coordinates.x, (int)coordinates.y, value);
    }

    public int GetValue(int x, int y)
    {
        if (isValid(x, y)) return gridArray[x, y];

        return -1;
    }

    public int GetValue(Vector3 worldPosition)
    {
        Vector2 coordinates = GetXY(worldPosition);
        return GetValue((int)coordinates.x, (int)coordinates.y);
    }

    private bool isValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    private Vector2 GetXY(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int y = Mathf.FloorToInt(worldPosition.y / cellSize);

        return new Vector2(x, y);
    }
}
