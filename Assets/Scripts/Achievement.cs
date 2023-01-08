using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "RPG/Achievement")]
public class Achievement : ScriptableObject, ISerializationCallbackReceiver
{
    public int ID;
    [SerializeField] private string achievementName;
    public EnemyData goal;
    public int[] amountForNextTier = new int[6];
    public RewardBonusList[] tierRewards = new RewardBonusList[6];
    public int CurrentAmount { get; private set; }
    public bool Earned { get; private set; }
    public int Tier { get; private set; }

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
        return $"{achievementName} {tiers[Tier]}";
    }

    public void IncreaseCurrentAmount() 
    { 
        if(CurrentAmount < amountForNextTier[Tier]) 
            CurrentAmount++;

        if (CurrentAmount == amountForNextTier[Tier])
            GetRewards();
    }

    private void GetRewards()
    {
        Player.Instance.Data.earnedAchievements++;
        foreach (var reward in tierRewards[Tier].rewards)
        {
            Player.Instance.Data.AddStat(reward.stat, reward.value);
        }
        if(Tier < tiers.Length - 1)
            Tier++;
        else
            Earned = true;
    }

    public void Load(int currentAmount, bool earned, int tier)
    {
        CurrentAmount = currentAmount;
        Earned = earned;
        Tier = tier;
    }

    public void OnAfterDeserialize()
    {
        CurrentAmount = 0;
        Earned = false;
        Tier = 0;
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