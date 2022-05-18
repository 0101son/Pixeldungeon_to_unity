using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Sprite[] sprites;
    
    private Vector2Int gridSize;
    private bool gridSet = false;
    public GameObject[,] tileGrid;

    //새로고침
    public void Refresh(in int[,] map, in bool[,] visited, in bool[,] heroFOV)
    {
        
        SpriteRenderer temp;
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                temp = tileGrid[x, y].GetComponent<SpriteRenderer>();
                Color debugColor = new Color(1f, 1f, 1f, 1);
                temp.sprite = sprites[map[x, y]];
                if (heroFOV[x, y] == false)
                    debugColor = new Color(0.7f, 0.7f, 0.7f, 1);
                if (visited[x, y] == false)
                    debugColor = new Color(0f, 0f, 0f, 1);
                
                temp.color = debugColor;
            }
        }

    }

    //초기화
    public void Initiate(in Vector2Int size)
    {
        //Debug.Log("Initiate");
        if (gridSet)

            DestroyGrid();

        gridSize = size;
        tileGrid = new GameObject[size.x, size.y];
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                tileGrid[j, i] = new GameObject("Tile[" + j + "," + i + "]");
                tileGrid[j, i].transform.position = new Vector3(j, i, 0);
                tileGrid[j, i].AddComponent<SpriteRenderer>();
            }
        }
        gridSet = true;
    }

    private void DestroyGrid()
    {
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                Destroy(tileGrid[j, i]);
            }
        }

        gridSet = false;
    }

    // Start is called before the first frame update

    // 타일 배치
    // 디스플레이 정보 입력
    // 새로고침
}
