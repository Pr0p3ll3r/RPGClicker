using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static int GetStat(Stats stat, ItemStat[] stats)
    {
        foreach(ItemStat _stat in stats)
        {
            if(_stat.stat == stat)
            {
                return _stat.currentValue;
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
}
