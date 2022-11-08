using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pet", menuName = "RPG/Items/Pet")]
public class Pet : Item
{
    [Header("Pet")]
    public int level = 1;
    public int exp;
    public int expToLvlUp = 10;
    public StatBonus[] stats;
    public int statsUnlocked = 1;

    private int maxLevel = 10;
    
    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public void AddExp(int amount)
    {
        if (level == maxLevel) return;
        exp += amount;
        CheckLvlUp();
    }

    private void CheckLvlUp()
    {
        expToLvlUp = 10 * level;

        while (exp >= expToLvlUp)
        {
            exp -= expToLvlUp;
            level++;
            statsUnlocked++;
            Debug.Log(itemName + ": Level Up! Current level: " + level);
        }
    }

    public void AddStats(PlayerData data)
    {
        foreach (StatBonus stat in stats)
        {
            if (stat != null)
                data.AddStat(stat.stat, stat.values[0]);
        }
    }

    public void RemoveStats(PlayerData data)
    {
        foreach (StatBonus stat in stats)
        {
            if (stat != null)
                data.RemoveStat(stat.stat, stat.values[0]);
        }
    }
}
