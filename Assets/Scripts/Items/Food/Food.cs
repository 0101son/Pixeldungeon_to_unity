using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    public static readonly int TIME_TO_EAT = 30;

    public int heal;

    public Food()
    {
        stackable = true;
        texture = "bread_ration";
    }

    public void Eat(Hero hero)
    {
        Detach(hero);
        UIManager.instance.UpdateUI();
        hero.Heal(heal);
        hero.Spend(TIME_TO_EAT);
    }
}
