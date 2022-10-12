using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "RPG/Item/Item")]
public class Item : ScriptableObject
{
    public string itemName = "Item";
    public Sprite icon = null;
    public ItemType itemType = ItemType.Item;
    public int quantity = 1;
    public int cost;

    public virtual void Use()
    {
        Debug.Log("Used " + name);
    }

    public void NewAsset(Item basic)
    {
        itemName = basic.itemName;
        name = itemName;
        itemType = basic.itemType;
        icon = basic.icon;
        cost = basic.cost;
    }
}

public enum ItemType
{ 
    Item,
    Equipment,
    Blueprint
}

[Serializable]
public class LootItem
{
    public Item item;
    [Range(0f, 1f)] public float chanceToDrop;
}

[Serializable]
public class NeededItem
{
    public Item item;
    public int quantity = 1;
}