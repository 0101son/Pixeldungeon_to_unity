using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{

    public static GameScene instance = null;

    private GameObject hero;

    public GameObject CharPrefab;
    public static Transform CharSpritesParent;


    public TileManager tileScript;
    public bool onControll = false;
    public static readonly float moveTime = 0.1f;
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

       
        Button Quit = GameObject.Find("Quit").GetComponent<Button>();
        Quit.onClick.AddListener(OnClickExit);

        tileScript = GetComponent<TileManager>();
        tileScript.Initiate(new Vector2Int(32, 32));

        StaticSetup();

        CharSpritesParent = GameObject.Find("CharSprites").transform;

        CharSprite.Link(Dungeon.hero);
        Dungeon.hero.sprite.focus = true;
        foreach(Mob mob in Dungeon.level.mobs)
        {
            CharSprite.Link(mob);
        }


    }

    //public static void Add(Mob mob) only for respawn

    private void StaticSetup()
    {
        //Debug.Log("GM StaticSetup");
        Level.StaticSetup();
        Actor.gameScript = this;
        CharSprite.Clear();
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

    public void OnClickExit()
    {
        Dungeon.SaveAll();
        SceneManager.LoadScene(0);
    }
}
