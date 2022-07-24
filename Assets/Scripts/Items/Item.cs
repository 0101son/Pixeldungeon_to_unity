using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	protected static readonly int TIME_TO_PICK_UP = 10;
	protected static readonly int TIME_TO_DROP = 10;

	public string texture = null;

	public bool stackable = false;
	public int quantity = 1;
	
	public bool DoPickUp(Hero hero)
	{
		if (Collect(hero.belongings))
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
		Dungeon.level.Drop(DetachAll(hero.belongings), position);
    }

	public Item Copy()
    {
		Item copy = new Item
		{
			quantity = quantity
			, texture = texture
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

	public Item Detach(Belongings container)
	{
		if (quantity <= 0)
		{
			return null;
		} else if (quantity == 1)
        {
			return DetachAll(container);
        }
        else
        {
			return Split(1);
		}
    }

	public Item DetachAll(Belongings container)
    {
		foreach(Item item in container.backpack.items)
		if(item == this)
        {
			container.backpack.items.Remove(this);
			return this;
        }

		return this;
    }

	public Item Merge(Item other)
    {
        if (IsSimilar(other))
        {
			quantity += other.quantity;
			other.quantity = 0;
        }
		return this;
    }

	public bool Collect(Belongings container)
    {
		Debug.Log("Hero Collect " + this);

		if(quantity <= 0)
        {
			return true;
        }

		List<Item> items = container.backpack.items;

		if(items.Contains(this))
        {
			return true;
        }

        if (!container.backpack.CanHold(this))
        {
			return false;
        }

		if (stackable)
		{
			foreach(Item item in items)
            {
				if (IsSimilar(item))
				{
					item.Merge(this);
					return true;
				}
			}
		}

		items.Add(this);

		return true;
    }

	public string Sprite()
    {
		return texture;
    }
	public override string ToString()
	{
		return GetType() + " * " + quantity;
	}
}
