using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : ItemSlot
{
    public InventorySlot(Item item) : base(item)
    {
    }

    public Item Item() { return item; }
}
