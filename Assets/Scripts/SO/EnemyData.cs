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
    public int criticalRate;

    public int exp;
    public int gold;
    public Loot[] loot;
    public bool isTowerMaster = false;
    public bool isBoss = false;
    public Achievement achievement;

    public Equipment[] equipment = new Equipment[16];

    public EnemyData GetCopy()
    {
        return Instantiate(this);
    }
}
