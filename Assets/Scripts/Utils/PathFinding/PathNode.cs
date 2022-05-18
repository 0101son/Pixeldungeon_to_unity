using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public Vector2Int Vector;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    private int terranCost;
    public PathNode cameFromNode;

    public PathNode(Vector2Int Vector, bool isWalkable)
    {
        this.Vector = Vector;
        this.isWalkable = isWalkable;
    }

    public void SetWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }

    public void SetTerranCost(int terranCost)
    {
        this.terranCost = terranCost;
    }

    public override string ToString()
    {
        return Vector.x + "," + Vector.y + "," + isWalkable;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}