using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string nickname;

    public int level;
    public int exp;
    public int remainPoints;
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

    public bool hardcore;
    public int currentTowerLevel;

    public PlayerData()
    {
        nickname = "BOB";
        level = 1;
        exp = 0;
        remainPoints = 0;
        strength = new Stat(0);
        intelligence = new Stat(0);
        dexterity = new Stat(0);
        damage = new Stat(0);
        defense = new Stat(0);
        maxHealth = new Stat(10);
        currentHealth = maxHealth.GetValue();
        criticalDamage = new Stat(50);
        criticalRate = new Stat(5);
        maxCriticalRate = new Stat(50);
        hardcore = false;
        currentTowerLevel = 0;
    }
}