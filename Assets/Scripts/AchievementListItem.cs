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
        achievementName.text = ach.GetAchievementName();
        achievementGoal.text = ach.goal.enemyName;
        achievementProgress.text = $"{ach.GetCurrentAmount()}/{ach.amountForNextTier[ach.GetTier()]}";
        for (int i = 0; i < rewardList.childCount; i++)
        {
            TextMeshProUGUI reward = rewardList.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>();
            reward.text = "";
            foreach (RewardBonus r in ach.tierRewards[ach.GetTier()].rewards)
            {              
                reward.text += $"+{r.value} {Utils.GetShortName(r.stat)}\n";
            }
            reward.text = reward.GetComponent<TextMeshProUGUI>().text.TrimEnd();
            if(ach.GetCurrentAmount() < ach.amountForNextTier[i])
                reward.color = Color.red;
            else
                reward.color = Color.green;
        }
        if (ach.GetEarned())
            gameObject.GetComponent<Image>().color = new Color32(0, 255, 44, 255);
    }

    public void RefreshProgress()      
    {
        achievementName.text = achievement.GetAchievementName();
        achievementProgress.text = $"{achievement.GetCurrentAmount()}/{achievement.amountForNextTier[achievement.GetTier()]}";
        for (int i = 0; i < rewardList.childCount; i++)
        {
            TextMeshProUGUI reward = rewardList.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>();
            if (achievement.GetCurrentAmount() < achievement.amountForNextTier[i])
                reward.color = Color.red;
            else
                reward.color = Color.green;
        }
        if (achievement.GetEarned())
            gameObject.GetComponent<Image>().color = new Color32(0, 255, 44, 255);
    }
}
