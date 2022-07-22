using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public bool active;

    public Image Image;
    private Item item;
    private Text text;
    private int slotID;

    // Start is called before the first frame update

    void Awake()
    {
        Image = GetComponent<Image>();

    }

    public  ItemSlot(int slotID)
    {
        this.slotID = slotID;
    }

    public void Clear()
    {
        Item(null);
    }

    public void Item(Item item)
    {
        this.item = item;

        if(item == null)
        {
            Enable(false);

        }
    }

    public void UpdateText()
    {
        if(item == null)
        {
            text.color = Color.clear;
            return;
        }
        else
        {
            text.color = Color.white;
        }

        if(item.stackable != true || item.quantity == 1)
        {
            text.text = "";
        }
        else
        {
            text.text = item.quantity.ToString();
        }
    }

    public void Enable(bool value)
    {
        active = value;

        Color alpha = value ? Color.white : Color.clear;
        Image.color = Color.clear;

    }

    public void LeftClick()
    {

    }

    public void RightClick()
    {
        
    }
}
