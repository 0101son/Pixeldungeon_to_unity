using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryClip : ActionClip
{
    public readonly int HPGain;
    public RecoveryClip(Char who, int HPGain) : base(who)
    {
        this.HPGain = HPGain;
    }

    public override string ToString()
    {
        return who + " gain " + HPGain + " HP";
    }
}
