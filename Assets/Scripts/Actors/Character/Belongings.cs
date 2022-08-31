using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belongings
{
    private Char owner;


    public class Backpack : IBundlable
    {

        public Char owner;

        public List<Item> items = new List<Item>();

        public int capacity = 30;
    
		public void Clear()
        {
			items.Clear();
        }

		private static readonly string ITEMS	= "inventory";

		public void StoreInBundle(Bundle bundle)
        {
			bundle.Put(ITEMS, items);
        }

		public void RestoreFromBundle(Bundle bundle)
		{
			foreach(IBundlable item in bundle.GetCollection(ITEMS))
            {
				if (item != null) ((Item)item).Collect(((Hero)owner).belongings);
            }
		}

		public bool CanHold(Item item)
        {
            if(items.Contains(item) || items.Count < capacity)
            {
                return true;
            } else if (item.stackable)
            {
                foreach(Item i in items)
                {
                    if (item.IsSimilar(i))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public Backpack backpack;

    public Weapon Weapon()
    {
        return weapon;
    }

	private static readonly string WEAPON	= "weapon";
	private static readonly string ARMOR	= "armor";
	private static readonly string ARTIFACT = "artifact";
	private static readonly string MISC     = "misc";
	private static readonly string RING     = "ring";

	public void StoreInBundle(Bundle bundle)
	{

		backpack.StoreInBundle(bundle);

		bundle.Put(WEAPON, weapon);
		//bundle.Put(ARMOR, armor);
		//bundle.Put(ARTIFACT, artifact);
		//bundle.Put(MISC, misc);
		//bundle.Put(RING, ring);
	}

	public void RestoreFromBundle(Bundle bundle)
	{

		backpack.Clear();
		backpack.RestoreFromBundle(bundle);

		weapon = (Weapon)bundle.GetBundlable(WEAPON);
	}

	/*
	public static void Preview(GameInProgress.Info info, Bundle bundle)
	{
		if (bundle.Contains(ARMOR))
		{
			Armor armor = ((Armor)bundle.get(ARMOR));
			if (armor instanceof ClassArmor){
				info.armorTier = 6;
			} else
			{
				info.armorTier = armor.tier;
			}
		}
		else
		{
			info.armorTier = 0;
		}
	}
	*/

	public Belongings(Char owner)
    {

        this.owner = owner;

        backpack = new Backpack();
        backpack.owner = owner;
    }

    public Weapon weapon = null;
    

}
