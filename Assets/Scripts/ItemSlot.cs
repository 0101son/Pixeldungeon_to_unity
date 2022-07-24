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

    private static readonly float ENABLED = 1.0f;
    private static readonly float DISABLED = 0.3f;
    // Start is called before the first frame update

    void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        bg = GetComponent<Image>();
    }

    public void Clear()
    {
        Item(null);
        Enable(true);
        itemImage.enabled = true;
    }

    public void Item(Item item)
    {
        Debug.Log(this.item);
        if(this.item == item)
        {
            if(item != null)
            {
                itemImage.enabled = true;
                itemImage.sprite = Load.Get(item.texture);
            }
            UpdateText();
            return;
        }

        this.item = item;

        if(item == null)
        {
            Enable(false);
            itemImage.enabled = false;

            UpdateText();
        }
        else
        {
            Enable(true);
            itemImage.enabled = true;

            itemImage.sprite = Load.Get(item.texture);
            UpdateText();
        }
    }

    public void UpdateText()
    {
        if(item == null)
        {
            text.enabled = false;
            return;
        }
        else
        {
            text.enabled = true;
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

        float alpha = value ? ENABLED : DISABLED;
        itemImage.color = new Color(itemImage.color.r,itemImage.color.g,itemImage.color.b,alpha);
    }

    public void Show(bool value)
    {
        bg.enabled = value;
        itemImage.enabled = value;
        text.enabled = value;

    }
}
