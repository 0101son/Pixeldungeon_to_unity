using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Mob
{
    public Skull() : base()
    {
        charID = 1;

        HP = HT = 2;

        attackSpeed = 10;
        damage = 1;

        moveSpeed = 10;
    
    }

    public override void Die()
    {
        Dungeon.level.items[position.x, position.y] += 1;
        base.Die();
    }
}
