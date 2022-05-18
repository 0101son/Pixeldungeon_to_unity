using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dungeon
{
    public static Hero hero;
    public static Level level;

	public static void Initiate()
	{
		Actor.Clear();
		
		hero = new Hero();
	}

	public static Level NewLevel()
	{
		Dungeon.level = null;
		Actor.Clear();

		Level level = new Level();

		level.Create(TerrainGenerator.GetSample());

		return level;
	}

	public static void SwitchLevel(Level level, Vector2Int position)
    {
		if (level.solid[position.x,position.y])
		{
			position = level.entrance;
		}

		Dungeon.level = level;
		Actor.Initiate();//여기만 쓰임
		hero.Initiate(position);

	}
}
