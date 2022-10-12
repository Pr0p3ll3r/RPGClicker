using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : ScriptableObject
{
    public string characterName;

    public int level = 1;
    public int exp;
    public int expToLvl = 0;
    public int remainPoints;

    public int maxHealth = 100;
    public int currentHealth = 100;

    public Stat strength;
    public Stat stamina;
    public Stat vitality;
    public Stat agility;
    public Stat luck;

    public Stat damage;
    public Stat defense;
    public Stat criticalDamage;
    public Stat criticalChance;

    public Equipment[] equipment = new Equipment[12];

    public virtual void Initialize()
    {
        currentHealth = maxHealth;
        expToLvl = level * 10 + 60;
    }

    public bool TakeDamage(int damage)
    {
        currentHealth -= damage;

        //Debug.Log(characterName + " takes " + damage + " damage.");
        if (currentHealth <= 0)
        {
            //Debug.Log(characterName + " died");
            return true;
        }
        else
            return false;
    }  

    public void NewAsset(Character basic)
    {
        name = basic.name;
        level = basic.level;
        exp = basic.exp;
        expToLvl = basic.expToLvl;
        remainPoints = basic.remainPoints;
        maxHealth = basic.maxHealth;
        currentHealth = basic.currentHealth;
        defense = basic.defense;
        damage = basic.damage;
        strength = basic.strength;
        stamina = basic.stamina;
        vitality = basic.vitality;
        agility = basic.agility;
        luck = basic.luck;
        criticalChance = basic.criticalChance;
        criticalDamage = basic.criticalDamage;
    }
}
