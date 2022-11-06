using System.Collections;
using System.Collections.Generic;
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

    public static bool Critical(int critRate, int critDmg, ref int damage)
    {
        int random = Random.Range(0, 100);
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
        int index = 0;
        for (int i = 1; i < name.Length; i++)
        {
            if (char.IsUpper(name[i]))
            {
                index = i;
                break;
            }            
        }
        if(index != 0)
            name = name.Insert(index, " ");
        return name;
    }
}
