using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Level : IBundlable
{
	//StaticSetup
	public static GameScene GameScript;
	public static PathFinding PF;

	protected Vector2Int size;

	public int[,] map; //지형 정보
	public bool[,] visited; //시야에 들어왔던 적이 있는 모든 구역

	public int viewDistance = 8; //시야(임시)

	public bool[,] blocking;
	public bool[,] losBlocking;
	public List<Item>[,] item;

	public bool[,] heroFOV; //현재 hero 시야

	public Vector2Int entrance;
	public Vector2Int exit;
	//
	public HashSet<Mob> mobs;
	//reset() -> 안씀

	public static void StaticSetup()
	{
		//Debug.Log("Level StaticSetup");
		GameScript = GameScene.instance;
		//Debug.Log("Level StaticSetup Success");
	}

	private static readonly string VERSION = "version";
	private static readonly string SIZE = "size";
	private static readonly string MAP = "map";
	private static readonly string VISITED = "visited";
	private static readonly string MAPPED = "mapped";
	private static readonly string TRANSITIONS = "transitions";
	private static readonly string LOCKED = "locked";
	private static readonly string ITEM = "item";
	private static readonly string PLANTS = "plants";
	private static readonly string TRAPS = "traps";
	private static readonly string CUSTOM_TILES = "customTiles";
	private static readonly string CUSTOM_WALLS = "customWalls";
	private static readonly string MOBS = "mobs";
	private static readonly string BLOBS = "blobs";
	private static readonly string FEELING = "feeling";

	public void Create(int[,] map)
	{
		Dungeon.level = this;
		mobs = new HashSet<Mob>();

		CharSprite.SetMoveTime(GameScene.moveTime);

		entrance = new Vector2Int(1, 1);
		exit = new Vector2Int(10, 10);

		this.map = map;

		size.x = map.GetLength(0);
		size.y = map.GetLength(1);

		losBlocking = new bool[size.x, size.y];// Can't See thrugh; ex) wall
		visited = new bool[size.x, size.y];
		blocking = new bool[size.x, size.y];// Can't go through; ex) Someone is standing on the tile
		heroFOV = new bool[size.x, size.y];
		item = new List<Item>[size.x, size.y];

		Debug.Log("Basic Setting");

		for (int y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				losBlocking[x, y] = (map[x, y] == 1);
				visited[x, y] = false;
				blocking[x, y] = false;
				item[x, y] = new List<Item>();
			}
		}

		Debug.Log("Basic Setting2");

		Debug.Log("HeroSpawn");
		CreateMobs();

	}

	public void SetSize(Vector2Int size)
    {
		this.size = size;

		map = new int[size.x, size.y];

		visited = new bool[size.x, size.y];

		heroFOV = new bool[size.x, size.y];

		blocking = new bool[size.x, size.y];
		losBlocking = new bool[size.x, size.y];

		item = new List<Item>[size.x, size.y];
	}

	public void RestoreFromBundle(Bundle bundle)
    {
		SetSize(bundle.GetVector2Int(SIZE));

		mobs = new HashSet<Mob>();
		
		map = bundle.Get2DArray<int>(MAP);

		visited = bundle.Get2DArray<bool>(VISITED);
		item = bundle.Get2DArray<List<Item>>(ITEM);
		ICollection<IBundlable> collection = bundle.GetCollection(MOBS);
		foreach(IBundlable m in collection)
        {
			Mob mob = (Mob)m;
			if(mob != null)
            {
				mobs.Add(mob);
            }
        }
    }

	public void StoreInBundle(Bundle bundle)
    {
		bundle.Put(SIZE, size);
		bundle.Put(MAP, map);
		bundle.Put(VISITED, visited);
		bundle.Put(MOBS, mobs);
		bundle.Put(ITEM, item);

}
	public void CreateMobs()
    {
		for(int i=0; i<4; i++)
			SpawnMob<Skull>(exit + new Vector2Int(i,0));
		for (int i = 4; i < 8; i++)
			SpawnMob<RedSkull>(exit + new Vector2Int(i, 0));
	}

	public Item Drop(Item item, Vector2Int cell)
    {
		List<Item> placeToDrop = this.item[cell.x, cell.y];
		if (item.stackable)
		{
			foreach (Item loot in placeToDrop)
			{
				if (loot.GetType() == item.GetType())
				{
					loot.quantity += item.quantity;
					return loot;
				}
			}
		}
		placeToDrop.Add(item);
		return item;
    }

	public void SpawnMob<T>(Vector2Int position) where T : Mob
	{
		//It contains method level.createMob() in java
		Mob mob = (T)Activator.CreateInstance(typeof(T));
		mob.position = position;
		mob.sleeping = true;
		mobs.Add(mob);

		Dungeon.level.blocking[position.x, position.y] = true;
	}

	public void UpdateFieldOfView(Vector2Int origin, bool onlyCastShadow)
	{
		//Debug.Log(origin);
		ShadowCaster.CastShadow(origin, heroFOV, visited, losBlocking, viewDistance);
        if (!onlyCastShadow)
        {
			Char.UpdateFOV();
			GameScene.instance.tileScript.Refresh(map, visited, heroFOV);
		}
		
		//Debug.Log("FOVupdate");
	}

	public int Distance(Vector2Int a, Vector2Int b)
    {
		return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }

	public List<PathNode> DetourPath(Vector2Int start, Vector2Int target)
    {
		bool[,] walkableMap = new bool[size.x,size.y];
		for (int y = 0; y < size.y; y++)
			for (int x = 0; x < size.x; x++)
				walkableMap[x, y] = !(blocking[x, y] || losBlocking[x, y]);
		walkableMap[target.x, target.y] = true;
		PF = new PathFinding(walkableMap);
		return PF.FindPath(start, target);
	}

	public List<PathNode> PassPath(Vector2Int start, Vector2Int target)
	{
		bool[,] walkableMap = new bool[size.x, size.y];
		for (int y = 0; y < size.y; y++)
			for (int x = 0; x < size.x; x++)
				walkableMap[x, y] = !losBlocking[x, y];
		walkableMap[target.x, target.y] = true;
		PF = new PathFinding(walkableMap);
		return PF.FindPath(start, target);
	}
}
