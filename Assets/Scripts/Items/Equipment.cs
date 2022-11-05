using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "RPG/Items/Equipment")]
public class Equipment : Item
{
    [Header("Equipment")]
    public EquipmentType equipmentTypeSlot;
    public EquipmentRarity rarity;
    public int lvlRequired = 1;
    public Grade normalGrade;
    public bool canBeUpgraded;
    public bool canBeExtremeUpgraded;
    public bool canBeDivineUpgraded;
    public bool canBeChaosUpgraded;
    public bool scrollsCanBeAdded;
    public int startPrice;
    public int[] upgradePrices;

    [Header("Armor and Weapon")]
    public Grade extremeGrade;
    public Grade divineGrade;
    public int scrollSlots = 2;
    public int maxScrollSlots = 3;
    public Scroll[] scrolls = new Scroll[3];

    [Header("Jewelry")]
    public Grade chaosGrade;

    public ItemStat[] CurrentNormalStats() { return normalGrade.stats; }
    public ItemStat[] CurrentExtremeStats() { return extremeGrade.stats; }
    public ItemStat[] CurrentDivineStats() { return divineGrade.stats; }
    public ItemStat[] CurrentChaosStats() { return chaosGrade.stats; }

    private static int MAX_NORMAL_GRADE = 20;
    private static int MAX_EXTREME_GRADE = 15;
    private static int MAX_DIVINE_GRADE = 15;
    private static int MAX_CAHOS_GRADE = 15;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public void AddStats(PlayerData data)
    {
        foreach(ItemStat stat in normalGrade.stats)
        {
            switch (stat.stat)
            {
                case Stats.Damage:
                    data.damage.AddModifier(stat.values[normalGrade.level]);
                    break;
                case Stats.Defense:
                    data.defense.AddModifier(stat.values[normalGrade.level]);
                    break;
                case Stats.Health:
                    data.maxHealth.AddModifier(stat.values[normalGrade.level]);
                    break;
                case Stats.CritDamage:
                    data.criticalDamage.AddModifier(stat.values[normalGrade.level]);
                    break;
                case Stats.CritRate:
                    data.criticalRate.AddModifier(stat.values[normalGrade.level]);
                    break;
            }
        }
    }

    public void RemoveStats(PlayerData data)
    {
        foreach (ItemStat stat in normalGrade.stats)
        {
            switch (stat.stat)
            {
                case Stats.Damage:
                    data.damage.RemoveModifier(stat.values[normalGrade.level]);
                    break;
                case Stats.Defense:
                    data.defense.RemoveModifier(stat.values[normalGrade.level]);
                    break;
                case Stats.Health:
                    data.maxHealth.RemoveModifier(stat.values[normalGrade.level]);
                    break;
                case Stats.CritDamage:
                    data.criticalDamage.RemoveModifier(stat.values[normalGrade.level]);
                    break;
                case Stats.CritRate:
                    data.criticalRate.RemoveModifier(stat.values[normalGrade.level]);
                    break;
            }
        }
    }

    public int GetSellPrice()
    {
        int sellPrice;
        sellPrice = startPrice;
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
            canBeUpgraded = false;
    }

    public void DivineUpgrade()
    {
        divineGrade.level++;
        if (divineGrade.level == MAX_DIVINE_GRADE)
            canBeUpgraded = false;
    }

    public void ChaosUpgrade()
    {
        chaosGrade.level++;
        if (chaosGrade.level == MAX_CAHOS_GRADE)
            canBeUpgraded = false;
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
    Head,
    Chest,
    Hands,
    Feet,
    Amulet,
    Ring,
    Bracelet,
    Belt,
    Epaulet,
    Artifact
}

[System.Serializable]
public class ItemStat
{
    public Stats stat;
    public string name;
    public int[] values;
}

[System.Serializable]
public class Grade
{
    public int level;
    public ItemStat[] stats;  
}
