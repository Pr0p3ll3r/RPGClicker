using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "RPG/Achievement")]
public class Achievement : ScriptableObject, ISerializationCallbackReceiver
{
    public int ID;
    public string achievementName;
    public EnemyData goal;
    public int amount;
    public RewardBonus[] rewards;
    private int currentAmount;
    private bool earned;
    public int GetCurrentAmount() { return currentAmount; }
    public bool GetEarned() { return earned; }

    public void IncreaseCurrentAmount() 
    { 
        if(currentAmount < amount) 
            currentAmount++;

        if (currentAmount == amount)
            GetRewards();
    }

    private void GetRewards()
    {
        earned = true;
        Player.Instance.data.earnedAchievements++;
        foreach (var reward in rewards)
        {
            Player.Instance.data.AddStat(reward.stat, reward.value);
        }
    }

    public void Load(int currentAmount, bool earned)
    {
        this.currentAmount = currentAmount;
        this.earned = earned;
    }

    public void OnAfterDeserialize()
    {
        currentAmount = 0;
        earned = false;
    }

    public void OnBeforeSerialize() { }
}

[System.Serializable]
public class RewardBonus
{
    public Stats stat;
    public int value;
}