using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackClip : ActionClip
{
    public readonly Char whom;
    public readonly int damage;

    public AttackClip(Char who, Char whom, int damage) : base(who)
    {
        this.whom = whom;
        this.damage = damage;
    }

    public override string ToString()
    {
        return who + " -> " + whom + ", damage: " + damage;
    }
}
