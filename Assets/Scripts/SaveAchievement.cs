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
    public SaveAchievement(int progress, bool earned, int tier)
    {
        this.amount = progress;
        this.earned = earned;
        this.tier = tier;
    }
}
