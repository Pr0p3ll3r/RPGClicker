using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "RPG/Database")]
public class DatabaseSO : ScriptableObject, ISerializationCallbackReceiver
{
    public Item[] items;
    public Location[] locations;
    public Location[] dungeons;
    public StatBonus[] rarityBonuses;
    public StatBonus[] scrollBonuses;
    public EnemyInfo[] towerEnemies;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {     
        for (int i = 0; i < items.Length; i++)
        {
            items[i].ID = i;
        }

        for (int i = 0; i < rarityBonuses.Length; i++)
        {
            rarityBonuses[i].ID = i;
        }

        for (int i = 0; i < scrollBonuses.Length; i++)
        {
            scrollBonuses[i].ID = i;
        }
    }

    public void ResetLocations()
    {
        for (int i = 1; i < locations.Length; i++)
        {
            locations[i].unlocked = false;
            locations[i].bossDefeated = false;
        }
    }

    public Item GetItemById(int _id)
    {
        if (_id >= items.Length)
            return null;

        return items[_id];
    }

    public StatBonus GetRarityBonusById(int _id)
    {
        if (_id >= rarityBonuses.Length)
            return null;

        return rarityBonuses[_id];
    }

    public StatBonus GetScrollBonusById(int _id)
    {
        if (_id >= scrollBonuses.Length)
            return null;

        return scrollBonuses[_id];
    }
}
