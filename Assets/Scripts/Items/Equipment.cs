using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "RPG/Items/Equipment")]
public class Equipment : Item
{
    [Header("Equipment")]
    public EquipmentType equipmentTypeSlot;
    public EquipmentRarity rarity;
    public int lvlRequired = 1;
    public bool canBeUpgraded;
    public bool canBeExtremeUpgraded;
    public bool canBeDivineUpgraded;
    public bool canBeChaosUpgraded;
    public bool scrollsCanBeAdded;
    public bool CanStillBeUpgraded()
    {
        if (normalGrade.level == normalGrade.maxLevel)
            return false;
        return true;
    }
    public bool CanStillBeExtremeUpgraded()
    {
        if (extremeGrade.level == extremeGrade.maxLevel)
            return false;
        return true;
    }
    public bool CanStillBeDivineUpgraded()
    {
        if (divineGrade.level == divineGrade.maxLevel)
            return false;
        return true;
    }
    public bool CanStillBeChaosUpgraded()
    {
        if (chaosGrade.level == chaosGrade.maxLevel)
            return false;
        return true;
    }
    public bool ScrollsCanStillBeAdded()
    {
        for (int i = 0; i < scrollsStat.Length; i++)
        {
            if (scrollsStat[i] == null)
                return true;
        }
        return false;
    }

    [Header("Armor and Weapon")]
    public StatBonus rarityBonus;
    public Grade normalGrade;
    public bool is2HWeapon;
    public Grade extremeGrade;
    public Grade divineGrade;
    public int UsedScrollsSlot()
    {
        int value = 0;
        for (int i = 0; i < scrollsStat.Length; i++)
        {
            if (scrollsStat[i] != null)
                value++;
        }
        return value;
    }
    public StatBonus[] scrollsStat = new StatBonus[2];

    [Header("Jewelry")]
    public Grade chaosGrade;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public void AddStats(PlayerData data)
    {
        if(normalGrade.level >= 0)
            foreach (ItemStat stat in normalGrade.stats)
                data.AddStat(stat.stat, stat.values[normalGrade.level]);
        if (canBeExtremeUpgraded && extremeGrade.level > 0)
            foreach (ItemStat stat in extremeGrade.stats)
                data.AddStat(stat.stat, stat.values[extremeGrade.level - 1]);
        if (canBeDivineUpgraded && divineGrade.level > 0)
            foreach (ItemStat stat in divineGrade.stats)
                data.AddStat(stat.stat, stat.values[divineGrade.level - 1]);
        if (canBeChaosUpgraded && chaosGrade.level > 0)
            foreach (ItemStat stat in chaosGrade.stats)
                data.AddStat(stat.stat, stat.values[chaosGrade.level - 1]);

        //add stats from rarity bonus
        if (rarity != EquipmentRarity.Common)
            data.AddStat(rarityBonus.stat, rarityBonus.values[(int)rarity]);

        //add stats from scrolls
        foreach (StatBonus stat in scrollsStat)
        {
            if (stat != null)
                if(is2HWeapon)
                    data.AddStat(stat.stat, stat.values[1]);
                else
                    data.AddStat(stat.stat, stat.values[0]);
        }
    }

    public void RemoveStats(PlayerData data)
    {
        if (normalGrade.level >= 0)
            foreach (ItemStat stat in normalGrade.stats)
                data.RemoveStat(stat.stat, stat.values[normalGrade.level]);
        if (canBeExtremeUpgraded && extremeGrade.level > 0)
            foreach (ItemStat stat in extremeGrade.stats)
                data.RemoveStat(stat.stat, stat.values[extremeGrade.level - 1]);
        if (canBeDivineUpgraded && divineGrade.level > 0)
            foreach (ItemStat stat in divineGrade.stats)
                data.RemoveStat(stat.stat, stat.values[divineGrade.level - 1]);
        if (canBeChaosUpgraded && chaosGrade.level > 0)
            foreach (ItemStat stat in chaosGrade.stats)
                data.RemoveStat(stat.stat, stat.values[chaosGrade.level - 1]); ;

        //remove stats from rarity bonus
        if (rarity != EquipmentRarity.Common)
            data.RemoveStat(rarityBonus.stat, rarityBonus.values[(int)rarity]);

        //remove stats from scrolls
        foreach (StatBonus stat in scrollsStat)
        {
            if (stat != null)
                if (is2HWeapon)
                    data.RemoveStat(stat.stat, stat.values[1]);
                else
                    data.RemoveStat(stat.stat, stat.values[0]);
        }
    }

    public int GetSellPrice()
    {
        int sellPrice;
        sellPrice = price;
        if (normalGrade.level > 0)
            sellPrice += normalGrade.prices[normalGrade.level - 1] / 2;
        if (extremeGrade.level > 0)
            sellPrice += extremeGrade.prices[extremeGrade.level - 1] / 2;
        if (divineGrade.level > 0)
            sellPrice += divineGrade.prices[divineGrade.level - 1] / 2;
        if (chaosGrade.level > 0)
            sellPrice += chaosGrade.prices[chaosGrade.level - 1] / 2;
        return sellPrice;
    }

    public void AddScroll(StatBonus stat)
    {
        for (int i = 0; i < scrollsStat.Length; i++)
        {
            if (scrollsStat[i] == null)
            {
                scrollsStat[i] = stat;
                break;
            }               
        }
    }
}

public enum EquipmentRarity
{
    Common,
    Uncommon,
    Epic,
    Legendary
}

public enum EquipmentType
{
    MainHand,
    OffHand,
    Helmet,
    Chest,
    Gloves,
    Boots,
    Amulet,
    Ring,
    Bracelet,
    Belt,
    Earring
}

[System.Serializable]
public class ItemStat
{
    public Stats stat;
    public int[] values;
}

[System.Serializable]
public class Grade
{
    public int level;
    public int maxLevel;
    public ItemStat[] stats;
    public int[] prices;
}
