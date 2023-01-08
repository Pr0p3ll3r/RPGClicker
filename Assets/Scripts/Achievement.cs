using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "RPG/Achievement")]
public class Achievement : ScriptableObject, ISerializationCallbackReceiver
{
    public int ID;
    [SerializeField] private string achievementName;
    public EnemyData goal;
    public int[] amountForNextTier = new int[6];
    public RewardBonusList[] tierRewards = new RewardBonusList[6];
    private int currentAmount;
    private bool earned;
    public int GetCurrentAmount() { return currentAmount; }
    public bool GetEarned() { return earned; }
    private int tier;
    public int GetTier() { return tier; }

    private readonly string[] tiers = new string[6]
    {
        "Newbie",
        "Specialist",
        "Expert",
        "Master",
        "Grandmaster",
        "Legendary"
    };

    public string GetAchievementName()
    {
        return $"{achievementName} {tiers[tier]}";
    }

    public void IncreaseCurrentAmount() 
    { 
        if(currentAmount < amountForNextTier[tier]) 
            currentAmount++;

        if (currentAmount == amountForNextTier[tier])
            GetRewards();
    }

    private void GetRewards()
    {
        Player.Instance.data.earnedAchievements++;
        foreach (var reward in tierRewards[tier].rewards)
        {
            Player.Instance.data.AddStat(reward.stat, reward.value);
        }
        if(tier < tiers.Length - 1)
            tier++;
        else
            earned = true;
    }

    public void Load(int currentAmount, bool earned, int tier)
    {
        this.currentAmount = currentAmount;
        this.earned = earned;
        this.tier = tier;
    }

    public void OnAfterDeserialize()
    {
        currentAmount = 0;
        earned = false;
        tier = 0;
    }

    public void OnBeforeSerialize() { }
}

[System.Serializable]
public class RewardBonus
{
    public Stats stat;
    public int value;
}

[System.Serializable]
public class RewardBonusList
{
    public RewardBonus[] rewards;
}