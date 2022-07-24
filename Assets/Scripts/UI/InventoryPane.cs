using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPane : MonoBehaviour
{
    public static InventoryPane instance = null;
    public GameObject InventorySlot;
    private Transform equipPane;
    private Transform bagPane;

    public List<InventorySlot> equipped;
    public List<InventorySlot> bagItems;

    private bool active = true;
    public void TurnVisible()
    {
        if(active == true)
        {
            active = false;
            GetComponent<Image>().enabled = false;
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).GetComponent<Image>().enabled = false;
        }
        else
        {
            active = true;
            GetComponent<Image>().enabled = true;
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).GetComponent<Image>().enabled = true;
        }
    }

    public InventoryPane()
    {
        
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Debug.Log("Awake");
        equipPane = transform.GetChild(0);
        bagPane = transform.GetChild(1);
        CreateChildren();
    }

    public void CreateChildren()
    {
        Object slotPrefab = Resources.Load("Prefabs/ItemSlot");
        Debug.Log("CreateChildren");
        for(int i=0; i<4; i++)
        {
            GameObject slot = (GameObject)Instantiate(slotPrefab, equipPane.transform);
            equipped.Add(slot.GetComponent<InventorySlot>());
        }

        for (int i = 0; i < 30; i++)
        {
            GameObject slot = (GameObject)Instantiate(slotPrefab, bagPane.transform);
            bagItems.Add(slot.GetComponent<InventorySlot>());
        }
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        Debug.Log("update");
        
        Belongings stuff = Dungeon.hero.belongings;
        
        equipped[0].Item(stuff.weapon == null ? null : stuff.weapon);
        
        List<Item> items = stuff.backpack.items;


        int j = 0;
        for(int i = 0; i<30; i++)
        {
            if (items.Count > j)
            {
                Debug.Log(bagItems[i]);
                Debug.Log(items[j]);
                bagItems[i].Item(items[j]);
                Debug.Log(bagItems[i]);
                Debug.Log(items[j]);

                j++;
            } else {
				bagItems[i].Item(null);
            }   
        }
    }

    public void Active()
    {
        active = !active;
        
        GetComponent<Image>().enabled = active;
        equipPane.GetComponent<Image>().enabled = active;
        bagPane.GetComponent<Image>().enabled = active;
        foreach(InventorySlot slot in equipped)
        {
            slot.Show(active);
        }

        foreach (InventorySlot slot in bagItems)
        {
            slot.Show(active);
        }
    }
}
