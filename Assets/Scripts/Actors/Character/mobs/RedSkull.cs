using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSkull : Mob
{
    public RedSkull() : base()
    {
        charID = 2;

        HP = HT = 1;

        attackSpeed = 10;
        damage = 1;

        moveSpeed = 5;
    }

    public override void Die()
    {
        Dungeon.level.Drop(new Item(Item.Type.Food, 1), position);
        base.Die();
    }

}
