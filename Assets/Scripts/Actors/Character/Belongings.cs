using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belongings
{
    private Char owner;


    public class Backpack
    {

        public Char owner;

        public List<Item> items = new List<Item>();

        public int capacity = 30;
    
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


    public Belongings(Char owner)
    {
        this.owner = owner;

        backpack = new Backpack();
        backpack.owner = owner;
    }

    public Weapon weapon = null;
    

}
