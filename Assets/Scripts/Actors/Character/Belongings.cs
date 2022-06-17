using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belongings
{
    private Char owner;

    public Item backpack;


    public Belongings(Char owner)
    {
        this.owner = owner;

        backpack = null;
        weapon = null;
    }

    public Weapon weapon = null;
}
