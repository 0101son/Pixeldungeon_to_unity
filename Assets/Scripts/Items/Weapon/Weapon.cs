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
    public new void DoDrop(Char character)
    {
        Debug.Log("Weapon.dodrop");
        if(!IsEquipped(character) || DoUnequip(character, false))
        {
            base.DoDrop(character);
        }
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

        if (hero.belongings.weapon == null || hero.belongings.weapon.DoUnequip(hero,true))
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

    public bool DoUnequip(Char character, bool collect)
    {
        //EquipableItem
        character.Spend(TIME_TO_EQUIP);

        if(character is Hero hero)
        {
            if (!Collect(hero.belongings) || !collect)
            {

                if (collect) Dungeon.level.Drop(this, hero.position);
            }

            //KindOfWeapon
            hero.belongings.weapon = null;


            return true;
        }

        return false;
    }
    
}
