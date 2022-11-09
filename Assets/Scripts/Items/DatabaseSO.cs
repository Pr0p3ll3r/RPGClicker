using System.Collections;
using System.Collections.Generic;
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
    public EquipmentUpgrade[] allUpgradeInfos;

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