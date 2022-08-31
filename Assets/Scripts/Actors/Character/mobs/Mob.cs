using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Char
{
    

    public Mob()
    {
        texture = "adder";

        actPriority = MOB_PRIO;
    }

    private readonly string SLEEP	= "sleep";
    public bool sleeping;

    public override void StoreInBundle(Bundle bundle)
    {

        base.StoreInBundle(bundle);

        bundle.Put(SLEEP, sleeping);
    }

    public override void RestoreFromBundle(Bundle bundle)
    {

        base.RestoreFromBundle(bundle);

        sleeping = bundle.Get<bool>(SLEEP);
    }

    

    public override bool MoveToward(Vector2Int dir)
    {
        
        return base.MoveToward(dir);

    }

    public override void MoveTo(Vector2Int target)
    {
        visible = Dungeon.level.heroFOV[target.x, target.y];
        base.MoveTo(target);
    }

    public override bool Act()
    {
        Vector2Int HPosition = Dungeon.hero.position;

        if (sleeping == true)
        {
            if (Dungeon.level.heroFOV[position.x, position.y] == true && Dungeon.level.Distance(HPosition, position) < 4)
            {
                sleeping = false;
                Spend(10);
            }
            else
                Spend(1);
        }
        else
        {
            Debug.Log(this + "'s turn");
            if (Dungeon.level.Distance(HPosition, position) == 1)
            {
                Attack(Dungeon.hero);
            }
            else
            {
                if (TryMoveTo(NextStepTo(HPosition)) == false)
                {
                    Spend(10);
                }
            }
        }
            

        return false;
    }
}

