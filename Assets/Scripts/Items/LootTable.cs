using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public static class LootTable 
{
    public static Item Drop(Loot[] enemyLoot)
    {
        Item droppedItem = null;
        float totalWeight = enemyLoot.Sum(item => item.weight);
        float roll = Random.Range(0f, totalWeight);
        foreach (Loot loot in enemyLoot)
        {
            //Debug.Log(roll);
            if (loot.weight >= roll)
            {
                if (loot.item == null) return null;

                Item item = loot.item.GetCopy();
                if(item.itemType == ItemType.Equipment)
                {                         
                    droppedItem = MakeEquipment(item);
                }
                else
                {
                    droppedItem = item;
                }
                break;
            }
            roll -= loot.weight;
        }
        return droppedItem;
    }

    public static ItemStack[] QuestReward(ItemStack[] rewardList)
    {
        ItemStack[] rewards = new ItemStack[rewardList.Length];
        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewardList[i].item.itemType == ItemType.Equipment)
            {
                rewards[i] = new ItemStack(MakeEquipment(rewardList[i].item.GetCopy()), rewardList[i].amount);
            }
            else
            {
                rewards[i] = new ItemStack(rewardList[i].item.GetCopy(), rewardList[i].amount);
            }
        }
        return rewards;
    }

    public static ItemStack QuestRewardRandom(ItemStack[] rewards)
    {
        ItemStack random = rewards[Random.Range(0, rewards.Length)];
        ItemStack reward = new ItemStack(random.item.GetCopy(), random.amount);
        if (reward.item.itemType == ItemType.Equipment)
        {
            reward.item = MakeEquipment(reward.item);
        }
        return reward;
    }

    private static Equipment MakeEquipment(Item item)
    {
        Equipment eq = (Equipment)item;
        if (!eq.canBeChaosUpgraded)
        {
            eq.rarity = RandomRarity();
            if (eq.rarity != EquipmentRarity.Common)
            {
                eq.rarityBonus = Database.data.rarityBonuses[Random.Range(0, Database.data.rarityBonuses.Length)];
            }
            eq.scrollsStat = new StatBonus[RandomNumberOfSlots()];
        }
        return eq;
    }

    private static EquipmentRarity RandomRarity()
    {
        var weights = new (float weight, EquipmentRarity rarity)[]
        {               
              (50, EquipmentRarity.Common),
              (30, EquipmentRarity.Uncommon),
              (10, EquipmentRarity.Epic),
              (1, EquipmentRarity.Legendary)
        };
        float total = weights.Sum(item => item.weight);
        float random = Random.Range(0f, total);
        foreach (var item in weights)
        {
            if(item.weight >= random)
            {
                return item.rarity;
            }

            random -= item.weight;
        }
        return EquipmentRarity.Common;
    }

    private static int RandomNumberOfSlots()
    {
        float twoSlotDrop = 10;
        float twoSlotDropBonus = Player.Instance.data.twoSlotDropBonus.GetValue() / 100;
        twoSlotDrop += twoSlotDropBonus * twoSlotDrop;
        var weights = new (float weight, int number)[]
        {
              (60, 0),
              (30, 1),
              (twoSlotDrop, 2),
        };
        float total = 0;
        foreach (var item in weights)
        {
            total += item.weight;
        }
        float random = Random.Range(0f, total);
        foreach (var item in weights)
        {
            if (item.weight >= random)
            {
                return item.number;
            }

            random -= item.weight;
        }
        return 0;
    }
}
