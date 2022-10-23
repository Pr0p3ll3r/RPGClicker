using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "RPG/Items/Equipment")]
public class Equipment : Item
{
    [Header("Equipment")]
    public EquipmentTypeSlot equipmentTypeSlot;
    public EquipmentType equipmentType;
    public EquipmentRarity rarity;
    public int lvlRequired = 1;
    public int grade = 0;
    public Grade[] normalUpgrades;

    [Header("Armor and Weapon")]
    public int extremeGrade = 0;
    public Grade[] extremeUpgrades;
    public int divineGrade = 0;
    public Grade[] divineUpgrades;
    public int slots = 2;
    public int maxSlots = 3;
    public ItemStat[] slotStats = new ItemStat[3];

    [Header("Jewelry")]
    public int chaosGrade = 0;
    public Grade[] chaosUpgrades;

    [Header("Shop")]
    public bool canBeSold;
    public bool canBeUpgraded;
    public int startPrice;
    public int[] upgradePrices;
    public float upgradeMultiplier;

    public ItemStat[] CurrentNormalStats(){ return normalUpgrades[grade].stats; }
    public ItemStat[] CurrentExtremeStats() { return extremeUpgrades[grade].stats; }
    public ItemStat[] CurrentDivineStats() { return divineUpgrades[grade].stats; }

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public void AddStats(PlayerData data)
    {
        foreach(ItemStat stat in normalUpgrades[grade].stats)
        {
            switch (stat.stat)
            {
                case Stats.Damage:
                    data.damage.AddModifier(stat.currentValue);
                    break;
                case Stats.Defense:
                    data.defense.AddModifier(stat.currentValue);
                    break;
                case Stats.Health:
                    data.maxHealth.AddModifier(stat.currentValue);
                    break;
                case Stats.CritDamage:
                    data.criticalDamage.AddModifier(stat.currentValue);
                    break;
                case Stats.CritRate:
                    data.criticalRate.AddModifier(stat.currentValue);
                    break;
            }
        }
    }

    public void RemoveStats(PlayerData data)
    {
        foreach (ItemStat stat in normalUpgrades[grade].stats)
        {
            switch (stat.stat)
            {
                case Stats.Damage:
                    data.damage.RemoveModifier(stat.currentValue);
                    break;
                case Stats.Defense:
                    data.defense.RemoveModifier(stat.currentValue);
                    break;
                case Stats.Health:
                    data.maxHealth.RemoveModifier(stat.currentValue);
                    break;
                case Stats.CritDamage:
                    data.criticalDamage.RemoveModifier(stat.currentValue);
                    break;
                case Stats.CritRate:
                    data.criticalRate.RemoveModifier(stat.currentValue);
                    break;
            }
        }
    }

    public int GetSellPrice()
    {
        int sellPrice;
        sellPrice = startPrice;
        for (int i = 0; i < grade; i++)
        {
            sellPrice += upgradePrices[i];
        }
        sellPrice /= 2;
        return sellPrice;
    }

    public void Upgrade()
    {
        grade++;
        if (grade == normalUpgrades.Length)
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
    Weapon,
    Armor,
    Jewelry
}

public enum EquipmentTypeSlot
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
public class Grade
{
    public int level;
    public ItemStat[] stats;
}
