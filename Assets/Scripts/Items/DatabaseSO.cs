using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "RPG/Database")]
public class DatabaseSO : ScriptableObject, ISerializationCallbackReceiver
{
    public Sprite emptySlot;
    public Item[] items;
    public Location[] locations;
    public Location[] dungeons;
    public StatBonus[] rarityBonuses;
    public StatBonus[] scrollBonuses;
    public EnemyInfo[] towerEnemies;
    public Quest[] quests;
    public Achievement[] achievements;
    public RebirthBonus[] rebirthBonuses;

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

        for (int i = 0; i < quests.Length; i++)
        {
            quests[i].ID = i;
        }

        for (int i = 0; i < achievements.Length; i++)
        {
            achievements[i].ID = i;
        }

        for (int i = 0; i < rebirthBonuses.Length; i++)
        {
            rebirthBonuses[i].ID = i;
        }
    }
}
