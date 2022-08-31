using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics
{
    public static int deepestFloor;

    public static void Reset()
    {
        deepestFloor = 0;
    }

    private static readonly string DEEPEST		= "maxDepth";

    public static void StoreInBundle(Bundle bundle)
    {
        bundle.Put(DEEPEST, deepestFloor);
    }

    public static void RestoreInBundle(Bundle bundle)
    {
        deepestFloor = bundle.Get<int>(DEEPEST);
    }
}
