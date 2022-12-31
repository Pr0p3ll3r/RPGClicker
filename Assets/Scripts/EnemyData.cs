using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "RPG/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public Sprite icon;
    public int level = 1;

    public int health = 10;
    public int damage;
    public int defense;
    public int criticalDamage;
    public int criticalChance;

    public int exp;
    public int gold;
    public Loot[] loot;
    public bool isTowerEnemy = false;
    public bool isBoss = false;

    public EnemyData GetCopy()
    {
        return Instantiate(this);
    }
}
