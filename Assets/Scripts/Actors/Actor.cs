using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : IBundlable
{
    private int time;

    private int id = 0;

    protected static readonly int HERO_PRIO = 0;
    protected static readonly int MOB_PRIO = -20;
    private static readonly int DEFAULT = -100;

    protected int actPriority = DEFAULT;

    public abstract bool Act();

    public void Spend(int time)
    {
        this.time += time;
    }

    protected int Time()
    {
        return time;
    }

    private static readonly string TIME    = "time";
	private static readonly string ID      = "id";

    public virtual void StoreInBundle(Bundle bundle)
    {
        bundle.Put(TIME, time);
        bundle.Put(ID, id);
    }

    public virtual void RestoreFromBundle(Bundle bundle)
    {
        time = bundle.Get<int>(TIME);
        int incomingID = bundle.Get<int>(ID);
        if (FindById(incomingID) == null)
        {
            id = incomingID;
        }
        else
        {
            id = nextID++;
        }
    }

    public int Id()
    {
        if(id > 0)
        {
            return id;
        }
        else
        {
            return (id = nextID++);
        }
    }

    // **********************
    // *** Static members ***
    // **********************

    private static HashSet<Actor> all = new HashSet<Actor>();
    public static GameScene gameScript;
    private static Dictionary<int, Actor> ids = new Dictionary<int, Actor>();
    private static int nextID = 1;

    private static int now = 0;

    public static int Now()
    {
        return now;
    }

    public static void Clear()
    {
        now = 0;

        all.Clear();

        ids.Clear();
    }

    public static void Initiate()
    {
        Add(Dungeon.hero);
        //Debug.Log("how many mobs? " + Dungeon.level.mobs.Count);
        foreach (Mob mob in Dungeon.level.mobs)
        {
            Add(mob);
        }
        
    }

    private static readonly string NEXTID = "nextid";

    public static void StoreNextID(Bundle bundle)
    {
        bundle.Put(NEXTID, nextID);
    }

    public static void RestoreNextID(Bundle bundle)
    {
        nextID = bundle.Get<int>(NEXTID);
    }

    public static void ResetNextID()
    {
        nextID = 1;
    }

    public static void Process()
    {
        Actor current = PickNextActor();

        //if(current.time != now)
        //{
            //GameScene.instance.endAnimationQueue = true;
        now = current.time;
            //return;
        //}

        if (current is Hero)
        {
            GameScene.instance.endAnimationQueue = true;
            GameScene.instance.onControll = true;
        }
            

        current.Act();

        //Debug.Log("processed " + current);
    }

    public static Actor PickNextActor()
    {
        int min = int.MaxValue;
        Actor next = null;
        //Debug.Log("how many Actors? " + All().Count);
        foreach (Actor actor in all)
        {
            //Debug.Log("time of actor ID " + actor + ": " + actor.time);
            if (actor.time < min
                || (actor.time == min && actor.actPriority > next.actPriority))
            {
                next = actor;
                min = actor.time;
            }
        }
        return next;
    }

    public static void Add(Actor actor)
    {
        Add(actor, now);
    }

    public static void AddDelayed(Actor actor, int delay)
    {
        Add(actor, now + delay);
    }

    private static void Add(Actor actor, int time)
    {
        if (all.Contains(actor))
        {
            return;
        }

        ids.Add(actor.Id(), actor);

        all.Add(actor);
        actor.time += time;
    }

    public static void Remove(Actor actor)
    {
        if(actor != null)
        {
            Debug.Log("remove sucess");
            all.Remove(actor);

            if(actor.id > 0)
            {
                ids.Remove(actor.id);
            }
        }
        else
        {
            Debug.Log("remove failed");
        }
    }

    public static Actor FindById( int id)
    {
        if (ids.ContainsKey(id))
        {
            return ids[id];
        }
        else
        {
            return null;
        }
    }

    public static HashSet<Actor> All()
    {
        return new HashSet<Actor>(all);
    }
}
