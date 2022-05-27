using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAction
{
    public Vector2Int dest;

	public class Move : HeroAction
	{
		public Move(Vector2Int dest)
		{
			this.dest = dest;
		}
	}

	public class PickUp : HeroAction
	{
		public PickUp(Vector2Int dest)
        {
			this.dest = dest;
		}
	}

	public class Attack : HeroAction
	{
		public Char target;
		public Attack(Char target)
		{
			this.target = target;
		}
	}
}
