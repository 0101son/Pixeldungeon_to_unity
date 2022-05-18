using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Char
{
    public Mob()
    {
        actPriority = MOB_PRIO;
    }

    public bool sleeping;
    public override void Initiate(Vector2Int initPosition)
    {
        
        base.Initiate(initPosition);
        visible = Dungeon.level.heroFOV[position.x, position.y];
        Dungeon.level.mobs.Add(this);
        sleeping = true;
        //Debug.Log("Enemy Spawn");
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

