using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LootTable 
{
    public static Item Drop(Loot[] enemyLoot)
    {
        Item droppedItem = null;
        int roll = Random.Range(0, 100);
        foreach (Loot loot in enemyLoot)
        {
            if (roll <= loot.dropChance)
            {
                Item item = loot.item.GetCopy();
                if(item.itemType == ItemType.Equipment)
                {
                    Equipment eq = (Equipment)item;
                    foreach (ItemStat stat in eq.CurrentNormalStats())
                    {
                        int random = Random.Range(0, stat.values.Length);
                        stat.currentValue = stat.values[random];
                        eq.rarity = (EquipmentRarity)random;                     
                        item.price += stat.currentValue;
                    }
                    droppedItem = eq;
                }
                else
                {
                    droppedItem = item;
                }
                break;
            }
        }
        return droppedItem;
    }
}
