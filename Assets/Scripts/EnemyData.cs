using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Enemy", menuName = "RPG/Enemy")]
public class EnemyData : ScriptableObject
{
    public Sprite look;
    public string enemyName;
    public EnemyType enemyType;
    public int level = 1;

    public int health = 10;
    public int damage;
    public int defense;
    public int criticalDamage;
    public int criticalChance;

    public int exp;
    public int gold;
    public Loot[] loot;
    public int possibleRarity;
    public bool towerEnemy = false;
    public Location bossLocation;

    public EnemyData GetCopy()
    {
        return Instantiate(this);
    }
}

public enum EnemyType
{
    Melee,
    Range,
    Boss
}
