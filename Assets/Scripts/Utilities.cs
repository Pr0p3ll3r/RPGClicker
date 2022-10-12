using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static bool Critical(Character ch, ref int damage)
    {
        float random = Random.value;
        float critChance = (float)ch.criticalChance.GetValue() / 100;
        if (random <= critChance)
        {
            float critDamageFloat = (float)ch.criticalDamage.GetValue() / 100 * damage;
            int critDamage = Mathf.RoundToInt(critDamageFloat);
            damage += critDamage;
            return true;
        }
        else return false;
    }
}
