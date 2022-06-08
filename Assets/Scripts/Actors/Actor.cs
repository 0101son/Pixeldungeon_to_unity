using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor
{
    private int time;

    protected static readonly int HERO_PRIO = 0;
    protected static readonly int MOB_PRIO = -20;

    protected int actPriority = default;

    public abstract bool Act();

    public void Spend(int time)
    {
        this.time += time;
    }

    protected int Time()
    {
        return time;
    }

    // **********************
    // *** Static members ***
    // **********************

    private static HashSet<Actor> all = new HashSet<Actor>();

    private static int now = 0;

    public static int Now()
    {
        return now;
    }

    public static void Clear()
    {
        now = 0;

        all.Clear();
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

        all.Add(actor);
        actor.time += time;
    }

    public static void Remove(Actor actor)
    {
        if(actor != null)
        {
            Debug.Log("remove sucess");
            all.Remove(actor);
        }
        else
        {
            Debug.Log("remove failed");
        }
    }

    public static HashSet<Actor> All()
    {
        return new HashSet<Actor>(all);
    }
}
