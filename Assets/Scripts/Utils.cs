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
        string name = stat.ToString();
        for (int i = 1; i < name.Length; i++)
        {
            if (char.IsUpper(name[i]))
            {
                name = name.Insert(i, " ");
                i++;
            }
        }
        return name;
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
            case Stats.CriticalRate:
                return "CRIT RT";
            case Stats.MaxCriticalRate:
                return "MAX CRIT RT";
            case Stats.Accuracy:
                return "ACC";
            case Stats.HPSteal:
                return "HP STEAL%";
            case Stats.HPStealLimit:
                return "HP STEAL LIMIT";
            case Stats.GoldBonus:
                return "GOLD DROP%";
            case Stats.TwoSlotItemDrop:
                return "TWO-SLOT DROP%";
            case Stats.ExpBonus:
                return "EXP%";
            case Stats.Strength:
                return "STR";
            case Stats.Intelligence:
                return "INT";
            case Stats.Dexterity:
                return "DEX";
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
}
