using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "RPG/Achievement")]
public class Achievement : ScriptableObject, ISerializationCallbackReceiver
{
    public int ID;
    [SerializeField] private string achievementName;
    public Tier[] tiers = new Tier[6];
    public bool differentEnemies = false;
    public int CurrentAmount { get; private set; }
    public bool Earned { get; private set; }
    public int Tier { get; private set; }

    private readonly string[] tierNames = new string[6]
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
        if (tiers.Length == 1)
            return $"{achievementName}";
        return $"{achievementName} {tierNames[Tier]}";
    }

    public void IncreaseCurrentAmount() 
    { 
        if(CurrentAmount < tiers[Tier].amount) 
            CurrentAmount++;

        if (CurrentAmount == tiers[Tier].amount)
            GetRewards();
    }

    private void GetRewards()
    {
        Player.Instance.Data.earnedAchievements++;
        foreach (var reward in tiers[Tier].rewards)
        {
            Player.Instance.Data.AddStat(reward.stat, reward.value);
        }
        if(Tier < tiers.Length - 1)
        {
            Tier++;
            if (differentEnemies)
                CurrentAmount = 0;
        }         
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
public class Tier
{
    public EnemyData goal;
    public int amount;
    public RewardBonus[] rewards;
}