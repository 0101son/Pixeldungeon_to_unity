using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Mob
{
    public Skull() : base()
    {
        texture = "alligator";

        charID = 1;

        HP = HT = 2;

        attackSpeed = 10;
        damage = 1;

        moveSpeed = 10;
    
    }

    public override void Die()
    {
        if(Random.Range(0,2) == 0)
            Dungeon.level.Drop(new Apple(), position);
        else
            Dungeon.level.Drop(new Weapon(), position);
        base.Die();
    }
}
