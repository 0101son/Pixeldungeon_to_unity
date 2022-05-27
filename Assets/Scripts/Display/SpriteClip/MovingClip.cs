using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingClip : SpriteClip
{
    public Vector2Int from;
    public Vector2Int to;
    public bool visible;

    public MovingClip(Vector2Int from, Vector2Int to, bool visible)
    {
        this.from = from;
        this.to = to;
        this.visible = visible;
    }

    public override string ToString()
    {
        return from + " -> " + to + ", visible: " + visible;
    }

}
