using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string nickname;

    public int level;
    public int exp;
    public int remainingPoints;
    public Stat strength;
    public Stat intelligence;
    public Stat dexterity;

    public int currentHealth;
    public Stat damage;
    public Stat defense;
    public Stat maxHealth;
    public Stat criticalDamage;
    public Stat criticalRate;
    public Stat maxCriticalRate;

    public int currentTowerLevel;

    public PlayerData()
    {
        nickname = "BOB";
        level = 1;
        exp = 0;
        remainingPoints = 0;
        strength = new Stat(0);
        intelligence = new Stat(0);
        dexterity = new Stat(0);
        damage = new Stat(0);
        defense = new Stat(0);
        maxHealth = new Stat(10);
        currentHealth = maxHealth.GetValue();
        criticalDamage = new Stat(20);
        criticalRate = new Stat(5);
        maxCriticalRate = new Stat(50);
        currentTowerLevel = 0;
    }

    public void AddStrength()
    {
        remainingPoints--;
        strength.AddModifier(1);
        damage.AddModifier(1);
    }

    public void AddIntelligence()
    {
        remainingPoints--;
        intelligence.AddModifier(1);
        maxHealth.AddModifier(1);
    }

    public void AddDexterity()
    {
        remainingPoints--;
        dexterity.AddModifier(1);
        defense.AddModifier(1);
        //add accuracy?
    }
}