using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryPane : MonoBehaviour
{
    public static InventoryPane instance = null;
    public GameObject InventorySlot;
    private Transform equipPane;
    private Transform bagPane;

    public List<InventorySlot> equipped;
    public List<InventorySlot> bagItems;

    private bool active;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        equipPane = transform.GetChild(0);
        bagPane = transform.GetChild(1);
        CreateChildren();
        Active(false);
    }

    public void CreateChildren()
    {
        Object slotPrefab = Resources.Load("Prefabs/ItemSlot");

        equipped = new List<InventorySlot>();
        for(int i=0; i<4; i++)
        {
            GameObject slot = (GameObject)Instantiate(slotPrefab, equipPane.transform);
            InventoryPaneSlot script  = slot.AddComponent<InventoryPaneSlot>();
            equipped.Add(script);
        }

        bagItems = new List<InventorySlot>();
        for (int i = 0; i < 30; i++)
        {
            GameObject slot = (GameObject)Instantiate(slotPrefab, bagPane.transform);
            InventoryPaneSlot script = slot.AddComponent<InventoryPaneSlot>();
            bagItems.Add(script);
        }

        UpdateInventory();
    }

    public void UpdateInventory()
    {
        Belongings stuff = Dungeon.hero.belongings;
        
        equipped[0].Item(stuff.weapon == null ? null : stuff.weapon);
        
        List<Item> items = stuff.backpack.items;


        int j = 0;
        for(int i = 0; i<30; i++)
        {
            if (items.Count > j)
            {
                bagItems[i].Item(items[j]);

                j++;
            } else {
				bagItems[i].Item(null);
            }   
        }
    }

    public void Active(bool value)
    {
        active = value;

        GetComponent<Image>().enabled = value;
        equipPane.GetComponent<Image>().enabled = value;
        bagPane.GetComponent<Image>().enabled = value;
        foreach (InventorySlot slot in equipped)
        {
            slot.Active(value);
        }

        foreach (InventorySlot slot in bagItems)
        {
            slot.Active(value);
        }
    }

    public void Active()
    {
        active = !active;
        Active(active);
        
    }

    private class InventoryPaneSlot : InventorySlot, IPointerClickHandler
    {

        public void OnPointerClick(PointerEventData eventData)
        {
            if (item == null) return;

            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if (item is Weapon weapon)
                {
                    if (weapon.IsEquipped(Dungeon.hero))
                    {
                        weapon.DoUnequip(Dungeon.hero, true);
                    }
                    else
                    {
                        weapon.DoEquip(Dungeon.hero);
                    }
                }

                if (item is Food food)
                {
                    food.Eat(Dungeon.hero);
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if(item is Weapon weapon && weapon.IsEquipped(Dungeon.hero))
                {
                    Debug.Log("dropping weapon from hand");
                    weapon.DoDrop(Dungeon.hero);
                }
                else
                {
                    item.DoDrop(Dungeon.hero);
                }
                
            }

            instance.UpdateInventory();
            GameScene.instance.endAnimationQueue = true;
            GameScene.instance.onControll = false;
        }

    }
}
