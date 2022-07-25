using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public static readonly int TIME_TO_EQUIP = 10;

    public int damageBonus = 1;
    
    public Weapon()
    {
        stackable = false;
        texture = "long_sword1";
    }

    public new bool IsEquipped( Char character )
    {
        if(character is Hero hero)
        {
           return hero.belongings.Weapon() == this;
        }
        else
        {
            return false;
        }
    } 

    public bool DoEquip(Hero hero)
    {
        DetachAll(hero.belongings);

        if (hero.belongings.weapon == null || hero.belongings.weapon.DoUnequip(hero))
        {

            hero.belongings.weapon = this;
            InventoryPane.instance.UpdateInventory();

            hero.Spend(TIME_TO_EQUIP);
            return true;
        }
        else
        {
            Collect(hero.belongings);
            return false;
        }
    }

    public bool DoUnequip(Hero hero)
    {
        hero.Spend(TIME_TO_EQUIP);

        if (!Collect(hero.belongings))
            Dungeon.level.Drop(this, hero.position);

        hero.belongings.weapon = null;
        return true;
    }
    
}
