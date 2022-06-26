using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public GameObject[] Backpack;
    public GameObject Weapon;
    public GameObject Armor;
    public GameObject Ring;
    public GameObject Artifact;

    public Image[] belongins; 

    public Sprite tomato;
    public Sprite ration;
    public Sprite sword;
    public Image belonging;
    public Image weapon;
    public Text text;
    

    void Awake()
    {
        //Debug.Log("GM Awake");
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        UpdateUI();
    }


    public void UpdateUI()
    {
        Item backpack = Dungeon.hero.belongings.backpack;
        Item weapon = Dungeon.hero.belongings.weapon;
        

        if(backpack == null)
        {
            belonging.color = new Color(1, 1, 1, 0);
            text.text = " ";
        }
        else
        {
            belonging.color = new Color(1, 1, 1, 1);

            if (backpack is Ration)
            {
                belonging.sprite = ration;
            }

            if (backpack is Tomato)
            {
                belonging.sprite = tomato;
            }

            if (backpack is Weapon)
            {
                belonging.sprite = sword;
            }

            if (backpack.stackable)
                text.text = backpack.quantity.ToString();
            else
                text.text = null;
        }

        

        Debug.Log("weapon: " + weapon);

        if (weapon == null)
        {
            this.weapon.color = new Color(1, 1, 1, 0);
        }
        else
        {
            this.weapon.color = new Color(1, 1, 1, 1);

            this.weapon.sprite = sword;
        }
    }
}
