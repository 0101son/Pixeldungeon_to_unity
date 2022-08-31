using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Char
{

    public Belongings belongings;


    public Hero()
    {
        texture = "hog";

        actPriority = HERO_PRIO;

        HP = HT = 8;

        belongings = new Belongings(this);
        
        charID = 0;
        attackSpeed = 10;
        damage = 1;
        moveSpeed = 10;

    }
    /*
    public override void Initiate(Vector2Int initPosition)
    {
        //Debug.Log(initPosition);
        //Debug.Log(Dungeon.level);
        base.Initiate(initPosition);
        visible = true;
        sprite.focus = true;
    }
    */
    public override void StoreInBundle(Bundle bundle)
    {
        base.StoreInBundle(bundle);

        belongings.StoreInBundle(bundle);
    }

    public override void RestoreFromBundle(Bundle bundle)
    {
        base.RestoreFromBundle(bundle);

        belongings.RestoreFromBundle(bundle);
    }
    public static void Preview(GameInProgress.Info info, Bundle bundle)
    {
        //info.level = bundle.getInt(LEVEL);
        //info.str = bundle.getInt(STRENGTH);
        //info.exp = bundle.getInt(EXPERIENCE);
        info.hp = bundle.Get<int>(TAG_HP);
        info.ht = bundle.Get<int>(TAG_HT);
        //info.shld = bundle.getInt(Char.TAG_SHLD);
        //info.heroClass = bundle.getEnum(CLASS, HeroClass.class );
		//info.subClass = bundle.getEnum(SUBCLASS, HeroSubClass.class );
		//belongings.Preview(info, bundle);
	}

public override bool MoveToward(Vector2Int dir)
    {
        return base.MoveToward(dir);
    }

    public override void MoveTo(Vector2Int target)
    {

        Dungeon.level.UpdateFieldOfView(target,false);

        base.MoveTo(target);

    }

    public override bool Act()
    {
        Debug.Log("Player Turn");
        Dungeon.level.UpdateFieldOfView(position,false);
        gameScript.onControll = true;
        return false;
    }

    public bool ActDrop() //backpack에 drop할 item이 있는지 확인 -> 있다면 drop 후 animation 끊기
    {
        if (belongings.backpack != null)
        {
            belongings.backpack.items[0].DoDrop(this);
            InventoryPane.instance.UpdateInventory();
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
        if (belongings.backpack.items[0] is Weapon weapon)
        {
            weapon.DoEquip(this);
            InventoryPane.instance.UpdateInventory();
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
                InventoryPane.instance.UpdateInventory();
                loot.RemoveAt(count - 1);
                gameScript.endAnimationQueue = true;
                return true;
            }
        }

        return false;

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
