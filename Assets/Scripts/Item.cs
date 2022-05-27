using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	//protected static readonly int TIME_TO_THROW = 10; not yet
	protected static readonly int TIME_TO_PICK_UP = 10;
	//protected static readonly int TIME_TO_DROP = 10;

	public int image;
	//public int icon;

	//public bool stackable = false;
	//protected int quantity = 1;

	public void DoPickUp(Hero hero)
	{
		if (hero.HP < hero.HT)
			hero.HP++;
		hero.Spend(TIME_TO_PICK_UP);
	}
}
