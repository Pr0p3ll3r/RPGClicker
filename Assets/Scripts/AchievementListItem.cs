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
        achievementProgress.text = $"{ach.CurrentAmount}/{ach.amountForNextTier[ach.Tier]}";
        for (int i = 0; i < rewardList.childCount; i++)
        {
            TextMeshProUGUI reward = rewardList.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>();
            reward.text = "";
            foreach (RewardBonus r in ach.tierRewards[ach.Tier].rewards)
            {              
                reward.text += $"+{r.value} {Utils.GetShortName(r.stat)}\n";
            }
            reward.text = reward.GetComponent<TextMeshProUGUI>().text.TrimEnd();
            if(ach.CurrentAmount < ach.amountForNextTier[i])
                reward.color = Color.red;
            else
                reward.color = Color.green;
        }
        if (ach.Earned)
            gameObject.GetComponent<Image>().color = new Color32(0, 255, 44, 255);
    }

    public void RefreshProgress()      
    {
        achievementName.text = achievement.GetAchievementName();
        achievementProgress.text = $"{achievement.CurrentAmount}/{achievement.amountForNextTier[achievement.Tier]}";
        for (int i = 0; i < rewardList.childCount; i++)
        {
            TextMeshProUGUI reward = rewardList.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>();
            if (achievement.CurrentAmount < achievement.amountForNextTier[i])
                reward.color = Color.red;
            else
                reward.color = Color.green;
        }
        if (achievement.Earned)
            gameObject.GetComponent<Image>().color = new Color32(0, 255, 44, 255);
    }
}
