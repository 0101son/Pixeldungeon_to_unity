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

    // Start is called before the first frame update
    void Awake()
    {
        Image = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<Text>();
    }

    public void OnClick()
    {
        Clear();
    }

    public void Clear()
    {
        SetItem(null);
    }

    public void SetItem(Item item)
    {
        this.item = item;
        UpdateText();
        Enable(!(item == null));
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
        Image.enabled = value;
        text.enabled = value;
    }
}
