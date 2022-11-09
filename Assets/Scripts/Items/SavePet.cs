[System.Serializable]
public class SavePet
{
    public int ID;
    public int level;
    public int exp;
    public int[] statsIds = new int[10];
    public int statsUnlocked = 1;
    public SavePet()
    {
        ID = -1;
        level = 1;
        exp = 0;
        statsIds = null;
        statsUnlocked = 1;
    }
    public SavePet(int _ID, int _level, int _exp, StatBonus[] _stats, int _statsUnlocked)
    {
        ID = _ID;
        level = _level;
        exp = _exp;
        for (int i = 0; i < _stats.Length; i++)
        {
            if (_stats[i] != null)
                statsIds[i] = _stats[i].ID;
            else
                statsIds[i] = -1;
        }
        statsUnlocked = _statsUnlocked;
    }
}
