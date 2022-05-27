using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Char
{
    public Hero()
    {
        actPriority = HERO_PRIO;
        charID = 0;

        HP = HT = 8;

        attackSpeed = 10;
        damage = 1;

        moveSpeed = 10;
    }

    private List<Mob> visibleEnemies;

    public override void Initiate(Vector2Int initPosition)
    {
        //Debug.Log(initPosition);
        //Debug.Log(Dungeon.level);
        base.Initiate(initPosition);
        visible = true;
        sprite.focus = true;
        visibleEnemies = new List<Mob>();
    }


    public override bool MoveToward(Vector2Int dir)
    {
        return base.MoveToward(dir);
    }

    public override void MoveTo(Vector2Int target)
    {
        
        Dungeon.level.UpdateFieldOfView(target);
        int floor = Dungeon.level.items[target.x, target.y];
        if (floor > 0)
        {
            int gain;
            if (HP + floor > HT)
            {
                gain = HT - HP;
                HP = HT;
            }
            else
            {
                gain = floor;
                HP += floor;
            }

            sprite.EnqueueClip(new RecoveryClip(this,gain));
            
            Dungeon.level.items[target.x, target.y] = 0;
            
        }

        base.MoveTo(target);

    }

    public void CheckVisibleMobs()
    {
        List<Mob> visible = new List<Mob>();

        bool newMob = false;

        foreach(Mob mob in Dungeon.level.mobs)
        {
            if (Dungeon.level.heroFOV[mob.position.x, mob.position.y])
            {
                visible.Add(mob);
                if (visibleEnemies.Contains(mob))
                {
                    newMob = true;
                }
            }
        }

        if (newMob)
        {
            Interrupt();
        }

        visibleEnemies = visible;
    }

    public override bool Act()
    {
        Debug.Log("Player Turn");
        Dungeon.level.UpdateFieldOfView(position);
        gameScript.onControll = true;
        return false;
    }

    public override void Attack(Char target)
    {
        base.Attack(target);
        gameScript.endAnimationQueue = true;
    }

    public override void Die()
    {
        Debug.Log("P: Hero's advanture stops");
        gameScript.endAnimationQueue = true;
        gameScript.IsHeroAlive = false;
        base.Die();
    }

    public int VisibleEnemies()
    {
        return visibleEnemies.Count;
    }

    public Mob VisibleEnemy(int index)
    {
        return visibleEnemies[index];
    }

    public void Interrupt()
    {

    }

    public void Rest()
    {
        Spend(10);
    }
}
