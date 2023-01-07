using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI achievementName;
    [SerializeField] private TextMeshProUGUI achievementGoal;
    [SerializeField] private TextMeshProUGUI achievementProgress;
    [SerializeField] private Transform rewardList;
    [SerializeField] private GameObject achievementRewardPrefab;

    private Achievement achievement;

    public void SetAchievement(Achievement ach)
    {
        achievement = ach;
        achievementName.text = ach.achievementName;
        achievementGoal.text = ach.goal.enemyName;
        achievementProgress.text = $"{ach.GetCurrentAmount()}/{ach.amount}";
        foreach(RewardBonus r in ach.rewards) 
        {
            GameObject reward = Instantiate(achievementRewardPrefab, rewardList);
            reward.GetComponent<TextMeshProUGUI>().text = $"+{r.value} {Utils.GetNiceName(r.stat)}";
        }
        if (ach.GetEarned())
            gameObject.GetComponent<Image>().color = new Color32(0, 255, 44, 255);
    }

    public void RefreshProgress()      
    {
        achievementProgress.text = $"{achievement.GetCurrentAmount()}/{achievement.amount}";
        if (achievement.GetEarned())
            gameObject.GetComponent<Image>().color = new Color32(0, 255, 44, 255);
    }
}
