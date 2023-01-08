[System.Serializable]
public class SaveAchievement
{
    public int amount;
    public bool earned;
    public int tier;
    public SaveAchievement()
    {
        amount = 0;
        earned = false;
        tier = 0;
    }
    public SaveAchievement(int amount, bool earned, int tier)
    {
        this.amount = amount;
        this.earned = earned;
        this.tier = tier;
    }
}
