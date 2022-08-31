using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

public static class Dungeon
{
    public static Hero hero;
    public static Level level;

	public static int depth;
	public static void Initiate()
	{
		Actor.Clear();
		Actor.ResetNextID();

		Statistics.Reset();

		depth = 1;

		hero = new Hero();
	}

	public static Level NewLevel()
	{
		
		Dungeon.level = null;
		Actor.Clear();
		Debug.Log("D: Actor Clear");
		Level level = new Level();
		Debug.Log("D: New Level");
		level.Create(TerrainGenerator.GetSample());
		Debug.Log("D: Getting sample");
		return level;
	}

	public static void SwitchLevel(Level level, Vector2Int position)
    {
		if (level.losBlocking[position.x,position.y])
		{
			position = level.entrance;
		}

		Dungeon.level = level;
		hero.position = position;

		Actor.Initiate();//여기만 쓰임

		Observe();
		SaveAll();
	}

	public static long seed = 12345678;
	private static readonly string SEED		= "seed";
	private static readonly string HERO		= "hero";
	private static readonly string DEPTH	= "depth";
	private static readonly string VERSION	= "version";
	private static readonly string LEVEL	= "level";


	public static void SaveGame(int save)
	{
		Bundle bundle = new Bundle();

		bundle.Put(SEED, seed);
		Debug.Log("putting hero : " + hero);
		bundle.Put(HERO, hero);
		bundle.Put(DEPTH, depth);

		Actor.StoreNextID(bundle);

		Bundle.BundleToFile(GameInProgress.GameFile(save), bundle);
	}

	public static void SaveLevel(int save)
    {
		Bundle bundle = new Bundle();
		bundle.Put(LEVEL, level);

		Bundle.BundleToFile(GameInProgress.DepthFile(save, depth), bundle);
    }

	public static void SaveAll()
	{
		if (hero != null && (hero.HP>0)) {
			SaveGame(GameInProgress.curSlot );
			SaveLevel(GameInProgress.curSlot );

			GameInProgress.Set(GameInProgress.curSlot, depth, seed, hero);

		}
	}

	public static void LoadGame(int save)
    {
		Bundle bundle = Bundle.BundleFormFile(GameInProgress.GameFile(save));

		seed = bundle.Get<long>(SEED);

		Actor.Clear();
		Actor.RestoreNextID(bundle);

		level = null;
		depth = -1;

		hero = null;
		hero = (Hero)bundle.GetBundlable(HERO);

		depth = bundle.Get<int>(DEPTH);
    }

	public static Level LoadLevel(int save)
    {
		Dungeon.level = null;
		Actor.Clear();

		Bundle bundle = Bundle.BundleFormFile(GameInProgress.DepthFile(save, depth));

		Level level = (Level)bundle.GetBundlable(LEVEL);

		return level;
    }

	public static void DeleteGame(int save, bool deleteLevels)
	{

		if (deleteLevels)
		{
			string folder = GameInProgress.GameFolder(save);
			foreach (string file in Directory.GetFiles(folder))
			{
				if (file.Contains("depth"))
				{
					File.Delete(file);
				}
			}
		}

		File.Delete(GameInProgress.GameFile(save));

		GameInProgress.Delete(save);
	}

	public static void Preview(GameInProgress.Info info, Bundle bundle)
	{
		info.depth = bundle.Get<int>(DEPTH);
		info.version = bundle.Get<int>(VERSION);
		info.seed = bundle.Get<long>(SEED);

		Hero.Preview(info, bundle.GetBundle(HERO));
		//Statistics.preview(info, bundle);

	}
	public static void Store(JObject bundle)
	{
		bundle.Add("MATT", bundle);
	}

	public static void Observe()
    {
		level.UpdateFieldOfView(hero.position,true);
	}
}
