using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Char
{
    public Hero()
    {
        visibleEnemies = new List<Mob>();
        belonging = null;

        actPriority = HERO_PRIO;
        charID = 0;

        HP = HT = 8;

        attackSpeed = 10;
        damage = 1;

        moveSpeed = 10;

    }

    public Item belonging;
    private List<Mob> visibleEnemies;


    public override void Initiate(Vector2Int initPosition)
    {
        //Debug.Log(initPosition);
        //Debug.Log(Dungeon.level);
        base.Initiate(initPosition);
        visible = true;
        sprite.focus = true;
    }


    public override bool MoveToward(Vector2Int dir)
    {
        return base.MoveToward(dir);
    }

    public override void MoveTo(Vector2Int target)
    {

        Dungeon.level.UpdateFieldOfView(target);

        base.MoveTo(target);

    }

    public void CheckVisibleMobs()
    {
        List<Mob> visible = new List<Mob>();

        bool newMob = false;

        foreach (Mob mob in Dungeon.level.mobs)
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

    public bool ActDrop()
    {
        if (belonging != null)
        {
            belonging.DoDrop(this);
            UIManager.instance.Change(belonging);
            gameScript.endAnimationQueue = true;
            return true;
        }
        else
        {
            return false;
        }
        

    }

    public bool ActPickUp()
    {
        List<Item> loot = Dungeon.level.item[position.x, position.y];
        int count = loot.Count;
        if (count != 0)
        {
            Item item = loot[count - 1];
            if (item.DoPickUp(this))
            {
                UIManager.instance.Change(belonging);
                loot.RemoveAt(count - 1);
                gameScript.endAnimationQueue = true;
                return true;
            }
        }

        return false;

    }

    public bool ActConsume()
    {
        if (HP == HT || belonging == null) return false;

        if (belonging.type == Item.Type.Food)
        {
            Heal(belonging.quantity * 1);
        }

        if (belonging.type == Item.Type.Potion)
        {
            Heal(belonging.quantity * 2);
        }

        belonging = null;
        UIManager.instance.Change(null);
        return true;
    }

    public override void Die()
    {
        Debug.Log("P: Hero's advanture stops");
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
