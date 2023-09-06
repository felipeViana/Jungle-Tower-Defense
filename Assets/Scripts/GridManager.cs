using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private PathNode[,] gridArray;
    private TextMesh[,] debugTextArray;

    private bool debug = true;

    public GridManager(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        this.gridArray = new PathNode[width, height];
        this.debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new PathNode(this, x, y);

                if (debug)
                {
                    debugTextArray[x, y] = Utils.CreateWorldText(gridArray[x, y].ToString(), Color.white, GetWorldPosition(x, y) + new Vector3(cellSize / 2, cellSize / 2));

                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                }
            }
        }

        if (debug)
        {
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public void SetNode(int x, int y, PathNode node)
    {
        if (isValid(x, y))
        {
            gridArray[x, y] = node;

            if (debug) debugTextArray[x, y].text = node.ToString();
        }
    }

    public void SetNode(Vector3 worldPosition, PathNode node)
    {
        Vector2 coordinates = GetXY(worldPosition);
        SetNode((int)coordinates.x, (int)coordinates.y, node);
    }

    public PathNode GetNode(int x, int y)
    {
        if (isValid(x, y)) return gridArray[x, y];

        return default(PathNode);
    }

    public PathNode GetNode(Vector3 worldPosition)
    {
        Vector2 coordinates = GetXY(worldPosition);
        return GetNode((int)coordinates.x, (int)coordinates.y);
    }

    private bool isValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public Vector2 GetXY(Vector3 worldPosition)
    {
        Vector3 actualPosition = worldPosition - originPosition;
        int x = Mathf.FloorToInt(actualPosition.x / cellSize);
        int y = Mathf.FloorToInt(actualPosition.y / cellSize);

        return new Vector2(x, y);
    }

    public void SetNodeUnWalkable(Vector3 worldPosition)
    {
        PathNode node = GetNode(worldPosition);
        if (node == null) return;

        node.isWalkable = false;

        SetNode(worldPosition, node);
    }

    public Vector3 CenterPositionOnCell(Vector3 position)
    {
        Vector2 XY = GetXY(position);
        Vector3 worldPosition = GetWorldPosition((int)XY.x, (int)XY.y);

        return worldPosition + Vector3.one * cellSize / 2;
    }
}
