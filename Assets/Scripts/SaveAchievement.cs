[System.Serializable]
public class SaveAchievement
{
    public int amount;
    public bool earned;
    public SaveAchievement()
    {
        amount = 0;
        earned = false;
    }
    public SaveAchievement(int progress, bool earned)
    {
        this.amount = progress;
        this.earned = earned;
    }
}
