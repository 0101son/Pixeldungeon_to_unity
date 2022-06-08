using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	protected static readonly int TIME_TO_PICK_UP = 10;
	protected static readonly int TIME_TO_DROP = 10;

	public Sprite sprite;
	public int quantity;
	public Type type;

	public enum Type
    {
		Potion,
		Food
    }

    public Item(Type type, int quantity)
    {
		this.type = type;
		this.quantity = quantity;
    }

	public bool DoPickUp(Hero hero)
	{
		if (Collect(hero))
		{
			hero.Spend(TIME_TO_PICK_UP);
			return true;
		}
		else
			return false;
	}

	public void DoDrop(Hero hero)
    {
		hero.Spend(TIME_TO_DROP);
		Vector2Int position = hero.position;
		Dungeon.level.Drop(this, position);
    }

	public bool Collect(Hero hero)
    {
		if(hero.belonging == null)
        {
			hero.belonging = this;

			return true;
        }
			
		if(hero.belonging.type == type)
        {
			hero.belonging.quantity += quantity;
			return true;
        }

		return false;
    }

	public override string ToString()
	{
		return type + " * " + quantity;
	}
}
