using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "RPG/Inventory")]
public class InventoryInfo : ScriptableObject
{
    public int gold;
    public List<Item> items = new List<Item>();
   
    public void Add(Item item)
    {
        Item ownedItem = HaveIt(item);

        if (ownedItem != null && ownedItem.quantity < 999)
            ownedItem.quantity++;
        else
        {
            item.quantity = 1;
            items.Add(item);
        }         
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public bool CheckItem(NeededItem nItem)
    {
        int need = nItem.quantity;

        foreach (Item i in items)
        {
            if (i.itemName == nItem.item.itemName)
            {
                int itemQuantity = i.quantity;
                need -= itemQuantity;
            }
        }

        if (need <= 0) return true;
        else return false;
    }

    public bool CheckGold(int neededGold)
    {
        if (gold >= neededGold)
        {
            gold -= neededGold;
            return true;
        } 
        else return false;
    }

    Item HaveIt(Item item)
    {
        if (item.itemType == ItemType.Equipment) return null;

        foreach(Item i in items)
        {
            if (i.itemName == item.itemName) return i;
        }
        return null;
    }

    public void Remove(Item item, int quantity)
    {
        foreach (Item i in items)
        {
            if (i.itemName == item.itemName)
            {
                int itemQuantity = i.quantity;
                itemQuantity = Mathf.Min(itemQuantity, quantity);
                quantity -= itemQuantity;
                i.quantity -= itemQuantity;

                if (i.quantity <= 0) Remove(i);
                if (quantity == 0) break;            
            }
        }
    }
}
