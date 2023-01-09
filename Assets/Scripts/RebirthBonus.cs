using UnityEngine;

[CreateAssetMenu(fileName = "RebirthBonus", menuName = "RPG/RebirthBonus")]
public class RebirthBonus : ScriptableObject, ISerializationCallbackReceiver
{
    public int ID;
    public string bonusName;
    public Sprite icon;
    public Stats stat;
    public RebirthBonusLevel[] levels;
    public int CurrentLevel { get; set; }
    public int CurrentValue { get; set; }

    public void OnAfterDeserialize()
    {
        CurrentLevel = 0;
        CurrentValue = 0;
    }

    public void OnBeforeSerialize() { }
}

[System.Serializable]
public class RebirthBonusLevel
{
    public int value;
    public int cost;
}
