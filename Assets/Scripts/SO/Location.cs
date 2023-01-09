using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "RPG/Location")]
public class Location : ScriptableObject, ISerializationCallbackReceiver
{
    public string locationName;
    public int price = 1;
    public int lvlMin = 1;
    public bool Unlocked { get; set; }
    public bool BossDefeated { get; set;}
    public bool isDungeon = false;
    public EnemyData[] enemies;
    public EnemyData boss;

    public void OnAfterDeserialize()
    {
        Unlocked = false;
        BossDefeated = false;
    }

    public void OnBeforeSerialize() { }
}