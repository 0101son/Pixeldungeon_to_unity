using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite food;
    public Sprite potion;

    private Vector2Int gridSize;
    private bool gridSet = false;
    public GameObject[,] tileGrid;
    public GameObject[,] itemGrid;

    //새로고침
    public void Refresh(in int[,] map, in bool[,] visited, in bool[,] heroFOV)
    {
        
        SpriteRenderer tempTile;
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                tempTile = tileGrid[x, y].GetComponent<SpriteRenderer>();
                Color debugColor = new Color(1f, 1f, 1f, 1);
                tempTile.sprite = sprites[map[x, y]];

                if (heroFOV[x, y] == false)
                    debugColor = new Color(0.7f, 0.7f, 0.7f, 1);
                if (visited[x, y] == false)
                    debugColor = new Color(1, 1, 1, 0);
                
                tempTile.color = debugColor;

                tempTile = itemGrid[x, y].GetComponent<SpriteRenderer>();
                if(heroFOV[x,y] == true)
                {
                    List<Item> loot = Dungeon.level.item[x, y];
                    if (loot.Count == 0)
                        tempTile.color = new Color(1f, 1f, 1f, 0);
                    else
                    {
                        if (loot[loot.Count-1].type == Item.Type.Potion)
                            tempTile.sprite = potion;
                        if (loot[loot.Count-1].type == Item.Type.Food)
                            tempTile.sprite = food;

                        tempTile.color = new Color(1f, 1f, 1f, 1);
                    }
                        
                }
                else
                {
                    if(visited[x,y] == true && tempTile.color.a == 1)
                    {
                        tempTile.color = new Color(1f, 1f, 1f, 0.5f);
                    }
                }

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
        itemGrid = new GameObject[size.x, size.y];
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                GameObject temp = new GameObject("Tile[" + j + "," + i + "]");

                temp.transform.position = new Vector3(j, i, 0);
                temp.AddComponent<SpriteRenderer>();
                tileGrid[j, i] = temp;

                GameObject ItemTemp = new GameObject("Item[" + j + "," + i + "]");

                ItemTemp.transform.position = new Vector3(j, i, -1);
                ItemTemp.AddComponent<SpriteRenderer>();
                SpriteRenderer tempRenderer = ItemTemp.GetComponent<SpriteRenderer>();
                tempRenderer.color = new Color(1, 1, 1, 0);
                tempRenderer.sprite = food;
                itemGrid[j, i] = ItemTemp;
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
