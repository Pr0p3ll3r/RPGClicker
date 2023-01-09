[System.Serializable]
public class SaveEquipment
{
    public int ID;
    public EquipmentRarity rarity;
    public int rarityBonusID;
    public int normalGradeLevel;
    public int extremeGradeLevel;
    public int divineGradeLevel;
    public int chaosGradeLevel;
    public int[] statsIds;
    public SaveEquipment()
    {
        ID = -1;
        rarity = 0;
        rarityBonusID = -1;
        normalGradeLevel = 0;
        extremeGradeLevel = 0;
        divineGradeLevel = 0;
        chaosGradeLevel = 0;
        statsIds = null;
    }
    public SaveEquipment(int _ID, EquipmentRarity _rarity, StatBonus _rarityBonus, int _normalGradeLevel, int _extremeGradeLevel, int _divineGradeLevel, int _chaosGradeLevel, StatBonus[] _stats)
    {
        ID = _ID;
        rarity = _rarity;
        if (_rarityBonus != null)
            rarityBonusID = _rarityBonus.ID;
        else
            rarityBonusID = -1;
        normalGradeLevel = _normalGradeLevel;
        extremeGradeLevel = _extremeGradeLevel;
        divineGradeLevel = _divineGradeLevel;
        chaosGradeLevel = _chaosGradeLevel;
        statsIds = new int[_stats.Length];
        for (int i = 0; i < _stats.Length; i++)
        {
            if (_stats[i] != null)
                statsIds[i] = _stats[i].ID;
            else
                statsIds[i] = -1;
        }
    }
}
