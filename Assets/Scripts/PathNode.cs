using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid grid;
    private int x;
    private int y;

    public int gCost; // walking cost from start node
    public int hCost; // heuristic cost to reach end node
    public int fCost;

    public PathNode cameFromNode;

    public PathNode(Grid grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public int GetX() { return x; }
    public int GetY() { return y; }

    public int CalculateFCost()
    {
        fCost = gCost + hCost;
        return fCost;
    }

    public override string ToString()
    {
        return x + ", " + y;
    }
}
