using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Level
{
	//StaticSetup
	public static GameScene GameScript;
	public static PathFinding PF;

	protected Vector2Int size;

	public int[,] map; //지형 정보
	public bool[,] visited; //시야에 들어왔던 적이 있는 모든 구역

	public int viewDistance = 8; //시야(임시)

	public bool[,] blocking;
	public bool[,] solid;
	public int[,] items;

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

	public void Create(int[,] map)
	{
		Dungeon.level = this;
		//Debug.Log("level initiated");
		mobs = new HashSet<Mob>();

		CharSprite.SetMoveTime(GameScript.moveTime);
		entrance = new Vector2Int(1, 1);
		exit = new Vector2Int(10, 10);

		this.map = map;
		
		size.x = map.GetLength(0);
		size.y = map.GetLength(1);

		solid = new bool[size.x, size.y];// Can't See thrugh; ex) wall
		visited = new bool[size.x, size.y];
		blocking = new bool[size.x, size.y];// Can't go through; ex) Someone is standing on the tile
		heroFOV = new bool[size.x, size.y];
		items = new int[size.x, size.y];

		for (int y = 0; y < size.y; y++)
		{
			for (int x = 0; x < size.x; x++)
			{
				solid[x, y] = (map[x, y] == 1);
				visited[x, y] = false;
				blocking[x, y] = false;
				items[x, y] = 0;
			}
		}
		Spawn(Dungeon.hero,entrance);
		CreateMobs();
		
	}

	public void CreateMobs()
    {
		for(int i=0; i<4; i++)
			Spawn(new Skull(), exit + new Vector2Int(i,0));
		for (int i = 4; i < 8; i++)
			Spawn(new RedSkull(), exit + new Vector2Int(i, 0));
	}

	public Mob CreateMob()
    {

		Mob enemy = new Skull();

		return enemy;
    }

	public void Spawn(Char toSpawn, Vector2Int position)
	{
		//Debug.Log("SpawnEnemy 1");
		toSpawn.Initiate(position);
		if(toSpawn is Mob mob)
			mobs.Add(mob);
	}

	public void UpdateFieldOfView(Vector2Int origin)
	{
		//Debug.Log(origin);
		ShadowCaster.CastShadow(origin, heroFOV, visited, solid, viewDistance);
		Char.UpdateFOV();
		GameScene.instance.tileScript.Refresh(map, visited, heroFOV);
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
				walkableMap[x, y] = !(blocking[x, y] || solid[x, y]);
		walkableMap[target.x, target.y] = true;
		PF = new PathFinding(walkableMap);
		return PF.FindPath(start, target);
	}

	public List<PathNode> PassPath(Vector2Int start, Vector2Int target)
	{
		bool[,] walkableMap = new bool[size.x, size.y];
		for (int y = 0; y < size.y; y++)
			for (int x = 0; x < size.x; x++)
				walkableMap[x, y] = !solid[x, y];
		walkableMap[target.x, target.y] = true;
		PF = new PathFinding(walkableMap);
		return PF.FindPath(start, target);
	}

	/*
	public IEnumerator MoveEnemies()
    {
		
		GameScript.enemiesMoving = true;

		foreach (Mob enemy in mobs)
        {
			enemy.Act();
			
		}

		yield return new WaitForSeconds(GameScript.moveTime);
		GameScript.onControll = true;

		GameScript.enemiesMoving = false;
	}
	*/
}
