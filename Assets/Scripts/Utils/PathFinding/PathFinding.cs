using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    public static PathFinding Instance { get; private set; }

    public UGrid<PathNode> Grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    private readonly Vector2Int[] neighborOffset =
    {
        new Vector2Int(-1,0),new Vector2Int(0,-1),new Vector2Int(0,1),new Vector2Int(1,0),
        new Vector2Int(-1,-1),new Vector2Int(-1,1),new Vector2Int(1,-1),new Vector2Int(1,1)
    };

    private readonly int[] terranCostBase =
    {
        1,// #0: Floor
        0,// #1: Wall
    };

    public PathFinding(bool[,] Map)
    {
        Vector2Int size = new Vector2Int(Map.GetLength(0), Map.GetLength(1));
        Grid = new UGrid<PathNode>(size, (Vector2Int position) => new PathNode(position,Map[position.x, position.y]));
    }

    public List<PathNode> FindPath(Vector2Int start, Vector2Int end)
    {
        PathNode startNode = Grid.GetGridObject(start);
        PathNode endNode = Grid.GetGridObject(end);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for(int x = 0; x < Grid.size.x; x++)
        {
            for(int y = 0; y < Grid.size.y; y++)
            {
                PathNode pathNode = Grid.GetGridObject(new Vector2Int(x,y));
                // gCost 초기화 
                pathNode.gCost = int.MaxValue;
                // fCost 초기화
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode,endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)
            {
                //Debug.Log("sucess");
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neibourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neibourNode))
                    continue;
                if (!neibourNode.isWalkable)
                {
                    closedList.Add(neibourNode);
                    continue;
                }

                int huristicGCost = currentNode.gCost + 1;

                if(huristicGCost < neibourNode.gCost)
                {
                    neibourNode.cameFromNode = currentNode;
                    neibourNode.gCost = huristicGCost;
                    neibourNode.hCost = CalculateDistance(neibourNode, endNode);
                    neibourNode.CalculateFCost();

                    if (!openList.Contains(neibourNode))
                    {
                        openList.Add(neibourNode);
                    }
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        foreach(Vector2Int offset in neighborOffset)
        {
            Vector2Int searchingVector = currentNode.Vector + offset;
            if(searchingVector.x >= 0 && searchingVector.y >= 0 && searchingVector.x < Grid.size.x && searchingVector.y < Grid.size.y)
            {
                neighbourList.Add(Grid.GetGridObject(searchingVector));
            }
        }
        return neighbourList;
    }

    private PathNode GetNode(Vector2Int index)
    {
        return Grid.GetGridObject(index);
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }
    private int CalculateDistance(PathNode a,PathNode b)
    {
        int xDistance = Mathf.Abs(a.Vector.x - b.Vector.x);
        int yDistance = Mathf.Abs(a.Vector.y - b.Vector.y);
        if (xDistance < yDistance)
            return xDistance;
        else
            return yDistance;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for(int i = 0; i<pathNodeList.Count; i++)
            if(pathNodeList[i].fCost < lowestFCostNode.fCost)
                lowestFCostNode = pathNodeList[i];
        return lowestFCostNode;
    }

    public void SetCost(UGrid<int> terranGrid)
    {

        for (int i = 0; i < terranGrid.size.y; i++)
        {
            for (int j = 0; j < terranGrid.size.x; j++)
            {
                if (terranCostBase[terranGrid.GetGridObject(new Vector2Int(j,i))] == 0)
                    this.Grid.GetGridObject(new Vector2Int(j, i)).SetWalkable(false);
                else
                {
                    this.Grid.GetGridObject(new Vector2Int(j, i)).SetWalkable(true);
                    this.Grid.GetGridObject(new Vector2Int(j, i)).SetTerranCost(terranCostBase[terranGrid.GetGridObject(new Vector2Int(j, i))]);
                }
                    
            }
        }

        
    }
}
