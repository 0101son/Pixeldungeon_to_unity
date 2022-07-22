using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belongings
{
    private Char owner;


    public class Backpack
    {
        public char owner;

        public List<Item> Items = new List<Item>();

        public int capacity = 20;
    }

    public Item backpack;


    public Belongings(Char owner)
    {
        this.owner = owner;

        backpack = null;
        weapon = null;
    }

    public Weapon weapon = null;

}
