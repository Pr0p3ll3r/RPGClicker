using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "RPG/Location")]
public class Location : ScriptableObject
{
    public string locationName;
    public int price = 1;
    public int lvlMin = 1;
    public bool unlocked = false;
    public bool bossDefeated = false;
    public bool isDungeon = false;
    public EnemyData[] enemies;
    public EnemyData boss;

    public Location GetCopy()
    {
        return Instantiate(this);
    }
}