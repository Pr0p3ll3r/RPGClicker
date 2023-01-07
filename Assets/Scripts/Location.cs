using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "RPG/Location")]
public class Location : ScriptableObject, ISerializationCallbackReceiver
{
    public string locationName;
    public int price = 1;
    public int lvlMin = 1;
    public bool unlocked = false;
    public bool bossDefeated = false;
    public bool isDungeon = false;
    public EnemyData[] enemies;
    public EnemyData boss;

    public void OnAfterDeserialize()
    {
        unlocked = false;
        bossDefeated = false;
    }

    public void OnBeforeSerialize() { }
}