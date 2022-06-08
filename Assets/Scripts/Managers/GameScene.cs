using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public static GameScene instance = null;
    public TileManager tileScript;
    public Canvas UI;
    public bool onControll = false;
    public readonly float moveTime = 0.2f;
    public bool spriteActing = false;
    public bool endAnimationQueue;
    public bool IsHeroAlive = true;

    void Awake()
    {
        //Debug.Log("GM Awake");
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        tileScript = GetComponent<TileManager>();
        tileScript.Initiate(new Vector2Int(32, 32));

        StaticSetup();

        Descend();
    }

    private void Descend()
    {
        //Debug.Log("Descending");
        if (Dungeon.hero == null)
        {
            Dungeon.Initiate();
        }
        //Debug.Log("Dungeon Initiated");
        Level level;
        //if(Dungeon.depth >= deepest)
        level = Dungeon.NewLevel();
        //Debug.Log("Dungeon New level");
        Dungeon.SwitchLevel(level, level.entrance);
        //Debug.Log("Dungeon Switched");
        Dungeon.level.UpdateFieldOfView(Dungeon.hero.position);
    }

    //public static void Add(Mob mob) only for respawn

    private void StaticSetup()
    {
        //Debug.Log("GM StaticSetup");
        Level.StaticSetup();
        Char.StaticSetup();
        //Debug.Log("GM StaticSetup Succsess");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("GM Start");

    }

    // Update is called once per frame
    void Update()
    {
        if (spriteActing || !IsHeroAlive || onControll) return;

        Debug.Log("G: Processing");

        while (endAnimationQueue == false)
            Actor.Process();

        Debug.Log("G: Playing Queue");

        StartCoroutine(CharSprite.PlayClipQueue());
        endAnimationQueue = false;
    }
}
