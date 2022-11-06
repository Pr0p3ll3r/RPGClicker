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
                    int randomRarity = UnityEngine.Random.Range(0, 100);
                    eq.rarity = RandomRarity(randomRarity);
                    int randomBonus = UnityEngine.Random.Range(0, Database.data.rarityBonuses.Length);
                    eq.rarityBonus = Database.data.rarityBonuses[randomBonus];
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

    public static EquipmentRarity RandomRarity(int roll)
    {
        switch (roll)
        {
            case 0:
                return EquipmentRarity.Legendary;
            case <= 5:
                return EquipmentRarity.Epic;
            case <= 30:
                return EquipmentRarity.Uncommon;
            default:
                return EquipmentRarity.Common;
        }
    }
}
