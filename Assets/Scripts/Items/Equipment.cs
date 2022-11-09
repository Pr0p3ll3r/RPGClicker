using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "RPG/Items/Equipment")]
public class Equipment : Item
{
    [Header("Equipment")]
    public EquipmentType equipmentTypeSlot;
    public EquipmentRarity rarity;
    public int lvlRequired = 1;
    public StatBonus rarityBonus;
    public Grade normalGrade;
    public bool canBeUpgraded;
    public bool canBeExtremeUpgraded;
    public bool canBeDivineUpgraded;
    public bool canBeChaosUpgraded;
    public bool scrollsCanBeAdded;
    public int[] upgradePrices;

    [Header("Armor and Weapon")]
    public bool is2HWeapon;
    public Grade extremeGrade;
    public Grade divineGrade;
    public int scrollSlots = 2;
    public int maxScrollSlots = 3;
    public StatBonus[] scrollsStat = new StatBonus[3];

    [Header("Jewelry")]
    public Grade chaosGrade;

    public static readonly int MAX_NORMAL_GRADE = 20;
    public static readonly int MAX_EXTREME_GRADE = 15;
    public static readonly int MAX_DIVINE_GRADE = 15;
    public static readonly int MAX_CHAOS_GRADE = 15;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public void AddStats(PlayerData data)
    {
        if(canBeUpgraded)
            foreach (ItemStat stat in normalGrade.stats)
                data.AddStat(stat.stat, stat.values[normalGrade.level]);
        if (canBeExtremeUpgraded)
            foreach (ItemStat stat in extremeGrade.stats)
                data.AddStat(stat.stat, stat.values[extremeGrade.level]);
        if (canBeDivineUpgraded)
            foreach (ItemStat stat in divineGrade.stats)
                data.AddStat(stat.stat, stat.values[divineGrade.level]);
        if (canBeChaosUpgraded)
            foreach (ItemStat stat in chaosGrade.stats)
                data.AddStat(stat.stat, stat.values[chaosGrade.level]);

        //add stats from rarity bonus
        if (rarity != EquipmentRarity.Common)
            data.AddStat(rarityBonus.stat, rarityBonus.values[(int)rarity]);

        //add stats from scrolls
        foreach (StatBonus stat in scrollsStat)
        {
            if (stat != null)
                data.AddStat(stat.stat, stat.values[0]);
        }
    }

    public void RemoveStats(PlayerData data)
    {
        if (canBeUpgraded)
            foreach (ItemStat stat in normalGrade.stats)
                data.RemoveStat(stat.stat, stat.values[normalGrade.level]);
        if (canBeExtremeUpgraded)
            foreach (ItemStat stat in extremeGrade.stats)
                data.RemoveStat(stat.stat, stat.values[extremeGrade.level]);
        if (canBeDivineUpgraded)
            foreach (ItemStat stat in divineGrade.stats)
                data.RemoveStat(stat.stat, stat.values[divineGrade.level]);
        if (canBeChaosUpgraded)
            foreach (ItemStat stat in chaosGrade.stats)
                data.RemoveStat(stat.stat, stat.values[chaosGrade.level]);

        //remove stats from rarity bonus
        if (rarity != EquipmentRarity.Common)
            data.RemoveStat(rarityBonus.stat, rarityBonus.values[(int)rarity]);

        //remove stats from scrolls
        foreach (StatBonus stat in scrollsStat)
        {
            if (stat != null)
                data.RemoveStat(stat.stat, stat.values[0]);
        }
    }

    public int GetSellPrice()
    {
        int sellPrice;
        sellPrice = price;
        for (int i = 0; i < normalGrade.level; i++)
        {
            sellPrice += upgradePrices[i];
        }
        sellPrice /= 2;
        return sellPrice;
    }

    public void Upgrade()
    {
        normalGrade.level++;
        if (normalGrade.level == MAX_NORMAL_GRADE)
            canBeUpgraded = false;       
    }

    public void ExtremeUpgrade()
    {
        extremeGrade.level++;
        if (extremeGrade.level == MAX_EXTREME_GRADE)
            canBeExtremeUpgraded = false;
    }

    public void DivineUpgrade()
    {
        divineGrade.level++;
        if (divineGrade.level == MAX_DIVINE_GRADE)
            canBeDivineUpgraded = false;
    }

    public void ChaosUpgrade()
    {
        chaosGrade.level++;
        if (chaosGrade.level == MAX_CHAOS_GRADE)
            canBeChaosUpgraded = false;
    }

    public void CheckOptions()
    {
        if (canBeUpgraded && normalGrade.level == MAX_NORMAL_GRADE)
            canBeUpgraded = false;
        if (canBeExtremeUpgraded && extremeGrade.level == MAX_EXTREME_GRADE)
            canBeExtremeUpgraded = false;
        if (canBeDivineUpgraded && divineGrade.level == MAX_DIVINE_GRADE)
            canBeDivineUpgraded = false;
        if (canBeChaosUpgraded && chaosGrade.level == MAX_CHAOS_GRADE)
            canBeChaosUpgraded = false;
        for (int i = 0; i < scrollSlots; i++)
        {
            if (scrollsStat[i] != null)
                continue;
            else
            {
                scrollsCanBeAdded = false;
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
    public ItemStat[] stats;  
}
