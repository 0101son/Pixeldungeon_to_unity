using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	protected static readonly int TIME_TO_PICK_UP = 10;
	protected static readonly int TIME_TO_DROP = 10;

	public Sprite sprite = null;

	public bool stackable = false;
	public int quantity = 1;
	
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

	public bool IsSimilar(Item item)
    {
		return GetType() == item.GetType();
    }

	public void DoDrop(Hero hero)
    {
		hero.Spend(TIME_TO_DROP);
		Vector2Int position = hero.position;
		Dungeon.level.Drop(DetachAll(hero), position);
    }

	public Item Copy()
    {
		Item copy = new Item
		{
			quantity = quantity
			, sprite = sprite
			, stackable = stackable
        };
        return copy;
    }

	public Item Split(int amount)
    {
		if(amount <= 0 || amount >= quantity)
        {
			return null;
        }
        else
        {
			Item split = Copy();

			split.quantity = amount;
			quantity -= amount;

			return split;
        }
    }

	public Item Detach(Hero hero)
	{
		if (quantity <= 0)
		{
			return null;
		}

		if (quantity == 1)
        {
			
			return DetachAll(hero);
        }
        else
        {
			return Split(1);
		}
    }

	public Item DetachAll(Hero hero)
    {
		if(hero.belongings.backpack == this)
        {
			hero.belongings.backpack = null;
			return this;
        }

		return this;
    }

	public bool Collect(Hero hero)
    {
		if(quantity <= 0)
        {
			return true;
        }

		if(hero.belongings.backpack == this)
        {
			return true;
        }

		if(hero.belongings.backpack == null)
        {
			hero.belongings.backpack = this;

			return true;
        }

		if (stackable)
		{
			if (IsSimilar(hero.belongings.backpack))
			{
				hero.belongings.backpack.quantity += quantity;
				return true;
			}
		}

		return false;
    }

	public override string ToString()
	{
		return GetType() + " * " + quantity;
	}
}
