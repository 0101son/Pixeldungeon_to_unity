using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public bool active;

    protected Item item;
    protected Image itemImage;
    protected Image bg;
    protected TextMeshProUGUI text;

    //private static readonly float ENABLED = 1.0f;
    //private static readonly float DISABLED = 0.3f;

    void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        bg = GetComponent<Image>();
        Clear();
    }

    public void Clear()
    {
        Item(null);
    }

    public void Item(Item item)
    {

        if(this.item == item)
        {
            if(item != null)
            {
                Show(item);
            }
            UpdateText();
            return;
        }

        this.item = item;

        if(item == null)
        {
            Show(null);
            UpdateText();
        }
        else
        {
            Show(item);
            UpdateText();
        }
    }

    public void UpdateText()
    {
        if(item == null || item.stackable != true || item.quantity == 1)
        {
            text.text = " ";
            return;
        }
        else
        {
            text.text = item.quantity.ToString();
        }
    }

    public void Show(Item item)
    {
        if(item == null)
        {
            itemImage.color = Color.clear;
        }
        else
        {
            itemImage.color = Color.white;
            itemImage.sprite = Load.Get(item.texture);
        }
        
    }

    /*
    public void Enable(bool value)
    {
        active = value;

        float alpha = value ? ENABLED : DISABLED;
        itemImage.color = new Color(itemImage.color.r,itemImage.color.g,itemImage.color.b,alpha);
    }
    */

    public void Active(bool value)
    {
        bg.enabled = value;
        itemImage.enabled = value;
        text.enabled = value;

    }
}
