using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Sprite food;
    public Sprite potion;
    public Image belonging;
    public Text text;
    

    void Awake()
    {
        //Debug.Log("GM Awake");
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        Change(null);
    }


    public void Change(Item item)
    {
        if(item == null)
        {
            belonging.color = new Color(1, 1, 1, 0);
            text.text = " ";
            return;
        }
        else
        {
            belonging.color = new Color(1, 1, 1, 1);
        }

        if(item.type == Item.Type.Potion)
        {
            belonging.sprite = potion;
        }

        if (item.type == Item.Type.Food)
        {
            belonging.sprite = food;
        }

        text.text = item.quantity.ToString();
    }
}
