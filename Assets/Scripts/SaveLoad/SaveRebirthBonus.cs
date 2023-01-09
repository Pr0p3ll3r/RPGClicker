[System.Serializable]
public class SaveRebirthBonus
{
    public int level;
    public int value;
    public SaveRebirthBonus()
    {
        level = 0;
        value = 0;
    }
    public SaveRebirthBonus(int level, int value)
    {
        this.level = level;
        this.value = value;
    }
}
