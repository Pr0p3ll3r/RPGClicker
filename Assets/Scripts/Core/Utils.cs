using UnityEngine;

public static class Utils
{
    public static int GetStat(Stats stat, ItemStat[] stats, int grade)
    {
        foreach(ItemStat _stat in stats)
        {
            if(_stat.stat == stat)
            {
                return _stat.values[grade];
            }
        }
        return 0;
    }

    public static bool Critical(int critDmg, int critRate, ref int damage, int maxCritRate = 100)
    {
        int random = Random.Range(0, 100);
        critRate = Mathf.Clamp(critRate, 0, maxCritRate);
        if (random <= critRate)
        {
            float critDamageFloat = (float)critDmg / 100 * damage;
            int critDamage = Mathf.RoundToInt(critDamageFloat);
            damage += critDamage;
            return true;
        }
        else return false;
    }

    public static string GetNiceName(Stats stat)
    {
        switch (stat)
        {
            case Stats.Damage:
                return "Damage";
            case Stats.Defense:
                return "Defense";
            case Stats.Health:
                return "Health";
            case Stats.CriticalDamage:
                return "Critical Damage";
            case Stats.CriticalRatePercent:
                return "Critical Rate";
            case Stats.MaxCriticalRatePercent:
                return "Max Critical Rate";
            case Stats.HPStealPercent:
                return "Health Steal";
            case Stats.HPStealLimit:
                return "Health Steal Limit";
            case Stats.GoldBonusPercent:
                return "Gold Bonus";
            case Stats.TwoSlotItemDropPercent:
                return "2-Slot Item Drop";
            case Stats.ExpBonusPercent:
                return "Exp Bonus";
            case Stats.Strength:
                return "Strength";
            case Stats.Intelligence:
                return "Intelligence";
            case Stats.Dexterity:
                return "Dexterity";
            case Stats.MaxPets:
                return "Max Pets";
            default:
                return "";
        }
    }

    public static string GetShortName(Stats stat)
    {
        switch (stat)
        {
            case Stats.Damage:
                return "DMG";
            case Stats.Defense:
                return "DEF";
            case Stats.Health:
                return "HP";
            case Stats.CriticalDamage:
                return "CRIT DMG";
            case Stats.CriticalRatePercent:
                return "CRIT RT";
            case Stats.MaxCriticalRatePercent:
                return "MAX CRIT RT";
            case Stats.HPStealPercent:
                return "HP ST";
            case Stats.HPStealLimit:
                return "HP ST LMT";
            case Stats.GoldBonusPercent:
                return "GOLD BON";
            case Stats.TwoSlotItemDropPercent:
                return "2-SLOT";
            case Stats.ExpBonusPercent:
                return "EXP BON";
            case Stats.Strength:
                return "STR";
            case Stats.Intelligence:
                return "INT";
            case Stats.Dexterity:
                return "DEX";
            case Stats.MaxPets:
                return "MAX PETS";
            default:
                return "";
        }
    }

    public static int GetRightSlot(Equipment eq)
    {
        EquipmentSlot[] slots = PlayerEquipment.Instance.Slots;
        for (int i = 0; i < slots.Length; i++)
        {
            if (eq.equipmentTypeSlot == slots[i].equipmentType && slots[i] == null)
                return i;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (eq.equipmentTypeSlot == slots[i].equipmentType)
                return i;
        }

        return -1;
    }

    public static Color SetColor(EquipmentRarity rarity)
    {
        switch (rarity)
        {            
            case EquipmentRarity.Uncommon:
                return Color.green;
            case EquipmentRarity.Epic:
                return new Color32(163, 53, 238, 255);
            case EquipmentRarity.Legendary:
                return new Color32(255, 165, 0, 255);
            default:
                return Color.white;
        }
    }
}
