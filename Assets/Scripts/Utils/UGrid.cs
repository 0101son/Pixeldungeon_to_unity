using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGrid<TGridObject>
{
    public Vector2Int size;
    private TGridObject[,] gridArray;
    public UGrid(Vector2Int size, Func<Vector2Int ,TGridObject> createGridObject)
    {
        this.size = size;

        gridArray = new TGridObject[size.x, size.y];

        for (int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y;y++)
            {
                gridArray[x, y] = createGridObject(new Vector2Int(x,y));
                //Debug.Log(gridArray[x, y]);
            }
        }

    }

    public void SetGridObject(Vector2Int index, TGridObject value)
    {
        if (index.x >= 0 && index.y >= 0 && index.x < size.x && index.y < size.y)
            gridArray[index.x, index.y] = value;
        else
            Debug.Log("Grid SetValue error!");
    }

    public TGridObject GetGridObject(Vector2Int index)
    {
        if (index.x >= 0 && index.y >= 0 && index.x < size.x && index.y < size.y)
            return gridArray[index.x, index.y];
        else
        {
            Debug.Log("Grid GetValue error!");
            return default(TGridObject);
        }
            
    }
}
