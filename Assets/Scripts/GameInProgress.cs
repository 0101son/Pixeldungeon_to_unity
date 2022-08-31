using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public class GameInProgress
{
    public static readonly int MAX_SLOTS = 4;

    private static Dictionary<int, Info> slotStates = new Dictionary<int, Info>();
    public static int curSlot;

    private static readonly string GAME_FOLDER = "game{0}";
    private static readonly string GAME_FILE = "game.json";
    private static readonly string DEPTH_FILE = "depth{0}.json";

    public static object lockObject = new object(); 

    public static bool GameExists(int slot)
    {
            //Debug.Log("Does Game exist??? " + slot);
            if (!Directory.Exists(GameFolder(slot))) return false;
            //Debug.Log("Folder exist " + slot);
            if (!File.Exists(GameFile(slot))) return false;
            //Debug.Log("File exist " + slot);
            return File.ReadAllLines(GameFile(slot)).Length > 1;
        
        
    }

    public static bool DepthExists(int slot, int depth)
    {
            //Debug.Log("Does Game exist??? " + slot);
            if (!Directory.Exists(GameFolder(slot))) return false;
            //Debug.Log("Folder exist " + slot);
            if (!File.Exists(DepthFile(slot, depth))) return false;
            //Debug.Log("File exist " + slot);
            return File.ReadAllLines(DepthFile(slot, depth)).Length > 1;
        
        
    }

    public static string GameFolder(int slot)
    {
            if (!Directory.Exists(Application.dataPath + '/' + string.Format(GAME_FOLDER, slot)))
            {
                Directory.CreateDirectory(Application.dataPath + '/' + string.Format(GAME_FOLDER, slot));
            }
            //Debug.Log(string.Format(Application.dataPath + '/' + GAME_FOLDER, slot));
            return string.Format(Application.dataPath + '/' + GAME_FOLDER, slot);
        
    }

    public static string GameFile(int slot)
    {
        return GameFolder(slot) + '/' + GAME_FILE;
    }

    public static string DepthFile(int slot, int depth)
    {
        return GameFolder(slot) + "/" + string.Format(DEPTH_FILE, depth);
    }

    public static int FirstEmpty()
    {
        for (int i = 1; i <= MAX_SLOTS; i++)
        {
            if (Check(i) == null) return i;
        }
        return -1;
    }

    public static List<Info> CheckAll()
    {
        List<Info> result = new List<Info>();
        for(int i = 1; i<= MAX_SLOTS; i++)
        {
            Info curr = Check(i);
            if (curr != null) result.Add(curr);
        }
        return result;
    }

    public static Info Check(int slot)
    {
        if (slotStates.ContainsKey(slot))
        {
            Debug.Log("ContainsKey" + slot);
            return slotStates[slot];
        }
        else if (!GameExists(slot))
        {
            Debug.Log("game DNE" + slot);
            slotStates.Add(slot, null);
            return null;
        }
        else
        {
                Debug.Log("Do not contains key BUT Game Exist" + slot);
                Bundle bundle = Bundle.BundleFormFile(GameFile(slot));
                Info info = new Info();
                info.slot = slot;

                slotStates.Add(slot, info);
                return info;
            
        }
    }

    public static void Set(int slot, int depth, long seed, Hero hero)
    {
        if (slotStates.ContainsKey(slot))
        {
            slotStates.Remove(slot);
        }

        Info info = new()
        {
            slot = slot,

            depth = depth,

            seed = seed,

            hp = hero.HP,
            ht = hero.HT
        };

        slotStates.Add(slot, info);
    }

    public static void Delete(int slot)
    {
        slotStates.Remove(slot);
        slotStates.Add(slot, null);
    }

    public class Info
    {
        public int slot;

        public int depth;
        public int version;

        public long seed;

        public int hp;
        public int ht;
    }
}
