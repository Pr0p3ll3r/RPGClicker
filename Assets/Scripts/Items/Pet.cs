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
    public bool LevelMaxed()
    {
        if (level == maxLevel)
            return true;
        return false;
    }
    public bool CanAddScroll()
    {
        for (int i = 0; i < statsUnlocked; i++)
        {
            if (scrollsStat[i] == null)
                return true;
        }

        return false;
    }
    private int maxLevel = 10;
    public static int BASE_REQUIRE_EXP = 1000;
    public int UsedScrollsSlot()
    {
        int value = 0;
        for (int i = 0; i < scrollsStat.Length; i++)
        {
            if (scrollsStat[i] != null)
                value++;
        }
        return value;
    }

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

    public void AddScroll(StatBonus stat)
    {
        for (int i = 0; i < statsUnlocked; i++)
        {
            if (scrollsStat[i] == null)
            {
                scrollsStat[i] = stat;
                break;
            }
        }
    }

    public void AddStats(PlayerData data)
    {
        foreach (StatBonus stat in scrollsStat)
        {
            if (stat != null)
                data.AddStat(stat.stat, stat.values[1]);
        }
    }

    public void RemoveStats(PlayerData data)
    {
        foreach (StatBonus stat in scrollsStat)
        {
            if (stat != null)
                data.RemoveStat(stat.stat, stat.values[1]);
        }
    }
}
