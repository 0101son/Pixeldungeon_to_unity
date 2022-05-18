using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionClip : SpriteClip
{
    public readonly Char who;

    public ActionClip(Char who)
    {
        this.who = who;

    }

    public override string ToString()
    {
        return who + "";
    }
}
