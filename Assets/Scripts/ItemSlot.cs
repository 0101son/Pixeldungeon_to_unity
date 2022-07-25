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
        Debug.Log("text: " + text);
        bg = GetComponent<Image>();
    }

    public ItemSlot(Item item)
    {
        Debug.Log("ItemSlot: executing self");
        Item(item);
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
                itemImage.sprite = Load.Get(item.texture);
            }
            UpdateText();
            return;
        }

        this.item = item;

        if(item == null)
        {
            itemImage.sprite = null;

            UpdateText();
        }
        else
        {
            Enable(true);

            itemImage.sprite = Load.Get(item.texture);
            UpdateText();
        }
    }

    public void UpdateText()
    {
        Debug.Log("updating... text: " + text);
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
