using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Char
{

    public Belongings belongings;


    public Hero()
    {
        actPriority = HERO_PRIO;

        HP = HT = 8;

        belongings = new Belongings(this);
        
        charID = 0;
        attackSpeed = 10;
        damage = 1;
        moveSpeed = 10;

    }

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

    public override bool Act()
    {
        Debug.Log("Player Turn");
        Dungeon.level.UpdateFieldOfView(position);
        gameScript.onControll = true;
        return false;
    }

    public bool ActDrop() //backpack에 drop할 item이 있는지 확인 -> 있다면 drop 후 animation 끊기
    {
        if (belongings.backpack != null)
        {
            belongings.backpack.DoDrop(this);
            UIManager.instance.UpdateUI();
            gameScript.endAnimationQueue = true;
            return true;
        }
        else
        {
            return false;
        }
        

    }

    public bool ActEquip()
    {
        if (belongings.backpack is Weapon weapon)
        {
            weapon.DoEquip(this);
            UIManager.instance.UpdateUI();
            gameScript.endAnimationQueue = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ActUnequip()
    {
        if (belongings.weapon is Weapon weapon)
        {
            weapon.DoUnequip(this);
            UIManager.instance.UpdateUI();
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
                UIManager.instance.UpdateUI();
                loot.RemoveAt(count - 1);
                gameScript.endAnimationQueue = true;
                return true;
            }
        }

        return false;

    }

    public bool ActConsume()
    {
        if (HP == HT || belongings.backpack == null) return false;

        if(belongings.backpack is Food food)
        {
            food.Eat(this);
        }
        return true;
    }

    public override void Die()
    {
        Debug.Log("P: Hero's advanture stops");
        gameScript.IsHeroAlive = false;
        base.Die();
    }

    public void Rest()
    {
        Spend(10);
    }
}
