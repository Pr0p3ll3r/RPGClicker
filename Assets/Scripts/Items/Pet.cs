using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pet", menuName = "RPG/Items/Pet")]
public class Pet : Item
{
    [Header("Pet")]
    public int level = 1;
    public int exp;
    public StatBonus[] scrollsStat = new StatBonus[10];
    public int statsUnlocked = 1;

    private int maxLevel = 10;
    public static int BASE_REQUIRE_EXP = 1000;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public void GetExp(int amount, PetInfo petInfo)
    {
        if (LevelMaxed()) return;

        exp += amount;
        petInfo.UpdateExpBar();
        CheckLevelUp(petInfo);
    }

    private void CheckLevelUp(PetInfo petInfo)
    {       
        if (level == maxLevel)
        {
            exp = 0;
            return;
        }

        int requireExp = BASE_REQUIRE_EXP * level;

        if (exp < requireExp)
            return;

        exp -= requireExp;
        level++;
        statsUnlocked++;
        Debug.Log(itemName + ": Level Up! Current level: " + level);
        petInfo.UpdateExpBar();
        CheckLevelUp(petInfo);
    }

    public bool LevelMaxed()
    {
        if (level == maxLevel)
            return true;
        return false;
    }

    public void AddStats(PlayerData data)
    {
        foreach (StatBonus stat in scrollsStat)
        {
            if (stat != null)
                data.AddStat(stat.stat, stat.values[2]);
        }
    }

    public void RemoveStats(PlayerData data)
    {
        foreach (StatBonus stat in scrollsStat)
        {
            if (stat != null)
                data.RemoveStat(stat.stat, stat.values[2]);
        }
    }
}
