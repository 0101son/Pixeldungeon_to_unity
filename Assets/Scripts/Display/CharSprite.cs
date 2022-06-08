using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSprite : MonoBehaviour
{
    LineRenderer lr;

    public static List<CharSprite> sprites = new List<CharSprite>();

    readonly int CHAR_Z_LAYER = -1;

    private Vector2Int position;
    public bool focus = false;

    private static float moveTime;
    private static CameraManager CameraScript;
    

    private SpriteRenderer spriteRenderer;
    private Queue<MovingClip> nonActionQueue;
    static private Queue<ActionClip> actionQueue;
    public bool visible;
    private int ID;
    private static bool nonActionQueueEmpty = true;
    public Char ch;
    public float HP;
    public float HT;

    public static IEnumerator PlayClipQueue()
    {
        Debug.Log("C: ---play Clip Queue---");
        GameScene.instance.spriteActing = true;
        if (nonActionQueueEmpty == false)
        {
            Debug.Log("C: play non action animation");
            PlayNonActionQueue();
            yield return new WaitForSeconds(moveTime);
            nonActionQueueEmpty = true;
        }
        
        int actionClipQueueCount = actionQueue.Count;

        for(int i = 0; i < actionClipQueueCount; i++)
        {
            ActionClip currentClip = actionQueue.Dequeue();
            Debug.Log("C: play action animation of " + currentClip.who);
            if(currentClip is AttackClip attackClip)
            {
                attackClip.who.sprite.Attack(attackClip, moveTime / 2);
                yield return new WaitForSeconds(moveTime/2);
            }
                
            if(currentClip is RecoveryClip recoveryClip)
            {
                recoveryClip.who.sprite.Recovery(recoveryClip, moveTime*1.25f);
                yield return new WaitForSeconds(moveTime/4);
            }
                
            
        }
        GameScene.instance.spriteActing = false;
    }

    private static void PlayNonActionQueue()
    {
        foreach (CharSprite sprite in sprites)
        {
            sprite.StartNonActionCoroutine();
        }
        
    }

    public void Awake()
    {
        CameraScript = CameraManager.instance;
        lr = GetComponent<LineRenderer>();
        lr.startWidth = .05f;
        lr.endWidth = .05f;
    }

    public void Initiate()
    {
        sprites.Add(this);
        nonActionQueue = new Queue<MovingClip>();
        actionQueue = new Queue<ActionClip>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Link(Char ch)
    {
        position = ch.position;
        HP = ch.HP;
        HT = ch.HT;
        SetVisible(Dungeon.level.heroFOV[position.x, position.y]);
        this.ch = ch;
        ID = ch.charID;
        ch.sprite = this;
        Place(ch.position);
        spriteRenderer.sprite = GameScene.instance.GetComponent<SpriteManager>().sprites[ID];
        //Debug.Log("CharSprite Linked ID:" + ID + ", position: " + position);
        if (ID == 0)
            CameraScript.transform.position = transform.position + Vector3.back;
    }

    public static void SetMoveTime(float time)
    {
        moveTime = time;
        //Debug.Log(moveTime);
    }

    public void EnqueueClip(SpriteClip clip)
    {
        if(clip is MovingClip movingClip)
        {
            Debug.Log("C: ID no." + ID + " Enqueued - " + movingClip);
            nonActionQueueEmpty = false;
            nonActionQueue.Enqueue(movingClip);
        }
        if (clip is ActionClip actionClip)
        {
            Debug.Log("C: ID no." + ID + " Enqueued - " + actionClip);
            actionQueue.Enqueue(actionClip);
            GameScene.instance.endAnimationQueue = true;

        }
    }

    private void StartNonActionCoroutine()
    {
        StartCoroutine(PlayNonActionClipQueue());
    }

    private IEnumerator PlayNonActionClipQueue()
    {
        int clipLen = nonActionQueue.Count;
        //Debug.Log("clip len: " + clipLen);
        float lengthPerclip = moveTime / clipLen;

        for (int i = 0; i < clipLen ; i++)
        {
            MovingClip clip = nonActionQueue.Dequeue();

            //Debug.Log("ID no." + ID + " Played - " + clip + "i: " + i);
            TurnTo(clip.from, clip.to);
            StartCoroutine(Walk(clip.from, clip.to, lengthPerclip));
            if (visible != clip.visible)
                StartCoroutine(TurnVisible(clip.visible, lengthPerclip));
            yield return new WaitForSeconds(lengthPerclip);
        }

    }

    private IEnumerator Walk(Vector2Int from, Vector2Int to, float clipLength)
    {
        if (focus == true)
            StartCoroutine(CameraScript.FocusOn(this, clipLength));
        float invClipLength = 1 / clipLength;
        Vector2Int dir = to - from;
        
        Vector3 end = new Vector3(to.x, to.y, CHAR_Z_LAYER);
        Vector3 movePerTime = new Vector3(dir.x, dir.y, 0) * invClipLength;

        float time = 0;
        while (time < clipLength)
        {
            //Debug.Log(spriteRenderer.color.a);
            float dt = Time.deltaTime;
            time += dt;
            transform.Translate(dt * movePerTime);
            yield return null;
        }
        transform.position = end;
    }

    private void Attack(AttackClip clip, float time)
    {
        TurnTo(clip.who.position, clip.whom.position);
        StartCoroutine(Attack(clip.who, clip.whom, clip.damage, time));
    }

    private IEnumerator Attack(Char who, Char whom, int damage , float clipLength)
    {
        float invClipLength = 1 / clipLength;
        Vector2Int dir = whom.position - who.position;

        Vector3 start = transform.position;
        Vector3 movePerTime = new Vector3(dir.x, dir.y, 0) * invClipLength;

        float time = 0;
        while (time < clipLength/2)
        {
            //Debug.Log(spriteRenderer.color.a);
            float dt = Time.deltaTime;
            time += dt;
            transform.Translate(dt * movePerTime);
            yield return null;
        }

        whom.sprite.HP -= damage;
        if (whom.sprite.HP <= 0)
        {
            whom.sprite.HP = 0;
            whom.sprite.Die();
        }
            

        while (time < clipLength)
        {
            //Debug.Log(spriteRenderer.color.a);
            float dt = Time.deltaTime;
            time += dt;
            transform.Translate(-dt * movePerTime);
            yield return null;
        }
        transform.position = start;
    }

    private void Recovery(RecoveryClip clip, float time)
    {
        StartCoroutine(Recovery(clip.who, clip.HPGain, time));
    }

    private IEnumerator Recovery(Char who, int HPGain, float clipLength)
    {
        float time = 0;

        who.sprite.HP += HPGain;

        float invClipLength = 0.5f / clipLength;
        while (time < clipLength)
        {
            float dt = Time.deltaTime;
            time += dt;
            //Debug.Log("ID no." + ID + "turning " + new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + floatVisible * dt * invClipLength));
            spriteRenderer.color = new Color(time * invClipLength + 0.5f, spriteRenderer.color.g, time * invClipLength + 0.5f);
            yield return null;
        }
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        Debug.Log(this + " die...");
        sprites.Remove(this);
        StartCoroutine(Death(moveTime));
    }

    private IEnumerator Death(float clipLength)
    {
        float time = 0;
        float invClipLength = 1 / clipLength;

        while (time < clipLength)
        {
            float dt = Time.deltaTime;
            time += dt;
            //Debug.Log("ID no." + ID + "turning " + new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + floatVisible * dt * invClipLength));
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1 - time * invClipLength);
            yield return null;
        }
        Destroy(gameObject);
    }
    private IEnumerator TurnVisible(bool visible, float clipLength)
    {
        //Debug.Log("ID no." + ID +", a is" + spriteRenderer.color.a + "now, turning visible to " + visible);
        float time = 0;
        float invClipLength = 1 / clipLength;
        float floatVisible;

        if (visible == true)
            floatVisible = 1;
        else
            floatVisible = -1;

        while (time < clipLength)
        {
            float dt = Time.deltaTime;
            time += dt;
            //Debug.Log("ID no." + ID + "turning " + new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + floatVisible * dt * invClipLength));
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + floatVisible * dt * invClipLength);
            yield return null;
        }
        SetVisible(visible);
    }

    private void Place(Vector2Int tile)
    {
        transform.position = new Vector3(tile.x, tile.y, CHAR_Z_LAYER);
    }


    public void SetVisible(bool visible)
    {
        this.visible = visible;
        if(visible == true)
            spriteRenderer.color = new Color(1,1,1,1);
        else
            spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    private void TurnTo(Vector2Int from, Vector2Int to)
    {
        if(ID == 0)
            if (to.x > from.x)
                spriteRenderer.flipX = false;
            else if (to.x < from.x)
                spriteRenderer.flipX = true;

        if(ID == 1)
            if (to.x < from.x)
                spriteRenderer.flipX = false;
            else if (to.x > from.x)
                spriteRenderer.flipX = true;
    }

    public static bool IsActionClipQueueEmpty()
    {
        return actionQueue.Count == 0;
    }

    public void Update()
    {
        if(visible == true)
        {
            lr.startColor = Color.red;
            lr.SetPosition(0, transform.position + new Vector3(-0.5f, -0.46875f, -0.5f));
            lr.SetPosition(1, transform.position + new Vector3(-0.5f + HP/HT, -0.46875f, -0.5f));
        }
        else
        {
            lr.SetPosition(0, Vector3.zero);
            lr.SetPosition(1, Vector3.zero);
        }
        


    }
}
