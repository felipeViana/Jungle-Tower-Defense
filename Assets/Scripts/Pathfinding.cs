using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(int width, int height)
    {
        grid = new Grid(width, height, 10f, Vector3.zero);
    }

    public Grid GetGrid() { return grid; }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetNode(startX, startY);
        PathNode endNode = grid.GetNode(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode node = grid.GetNode(x, y);
                node.gCost = int.MaxValue;
                node.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // reached goal
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) openList.Add(neighbourNode);
                }
            }
        }

        // out of nodes on the openList
        // can't reach destination!
        return null;
    }

    private List<PathNode> GetNeighbourList (PathNode currentNode)
    {
        List<PathNode > neighbourList = new List<PathNode>();

        if (currentNode.GetX() - 1 >= 0)
        {
            // Left
            neighbourList.Add(grid.GetNode(currentNode.GetX() - 1, currentNode.GetY()));

            // Left Down
            if (currentNode.GetY() - 1 >= 0) neighbourList.Add(grid.GetNode(currentNode.GetX() - 1, currentNode.GetY() - 1));

            // Left Up
            if (currentNode.GetY() + 1 < grid.GetHeight()) neighbourList.Add(grid.GetNode(currentNode.GetX() - 1, currentNode.GetY() + 1));
        }
        if (currentNode.GetX() + 1 < grid.GetWidth())
        {
            // Right
            neighbourList.Add(grid.GetNode(currentNode.GetX() + 1, currentNode.GetY()));

            // Right Down
            if (currentNode.GetY() - 1 >= 0) neighbourList.Add(grid.GetNode(currentNode.GetX() + 1, currentNode.GetY() - 1));

            // Right Up
            if (currentNode.GetY() + 1 < grid.GetHeight()) neighbourList.Add(grid.GetNode(currentNode.GetX() + 1, currentNode.GetY() + 1));
        }

        // Down
        if (currentNode.GetY() - 1 >= 0) neighbourList.Add(grid.GetNode(currentNode.GetX(), currentNode.GetY() - 1));
        
        // Up
        if (currentNode.GetY() + 1 < grid.GetHeight()) neighbourList.Add(grid.GetNode(currentNode.GetX(), currentNode.GetY() + 1));

        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode > path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        
        while (currentNode.cameFromNode != null) 
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.GetX() - b.GetX());
        int yDistance = Mathf.Abs(a.GetY() - b.GetY());
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> nodes)
    {
        PathNode lowestFCostNode = nodes[0];

        for (int i = 1; i < nodes.Count; i++)
        {
            if (nodes[i].fCost < lowestFCostNode.fCost) lowestFCostNode = nodes[i];
        }

        return lowestFCostNode;
    }
}
