using System;
using System.Collections;
using System.Collections.Generic;

public static class LootTable 
{
    public static Item Drop(Loot[] enemyLoot)
    {
        Item droppedItem = null;
        int roll = UnityEngine.Random.Range(0, 100);
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
                        int random = UnityEngine.Random.Range(0, Enum.GetValues(typeof(EquipmentRarity)).Length);
                        eq.rarity = (EquipmentRarity)random;                     
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
