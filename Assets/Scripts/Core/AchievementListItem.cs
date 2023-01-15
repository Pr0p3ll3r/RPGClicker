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
        achievementGoal.text = ach.tiers[ach.Tier].goal.enemyName;
        achievementProgress.text = $"{ach.CurrentAmount}/{ach.tiers[ach.Tier].amount}";
        foreach (Transform c in rewardList)
        {
            c.gameObject.SetActive(false);
        }
        for (int i = 0; i < ach.tiers.Length; i++)
        {
            TextMeshProUGUI reward = rewardList.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>();
            reward.text = "";
            foreach (RewardBonus r in ach.tiers[i].rewards)
            {              
                if(r.stat.ToString().Contains("Percent"))
                    reward.text += $"+{r.value}% {Utils.GetShortName(r.stat)}\n";
                else
                    reward.text += $"+{r.value} {Utils.GetShortName(r.stat)}\n";
            }
            reward.text = reward.GetComponent<TextMeshProUGUI>().text.TrimEnd();
            if(ach.Tier <= i && ach.CurrentAmount != ach.tiers[ach.Tier].amount)
                reward.color = Color.red;
            else
                reward.color = Color.green;
            if(ach.tiers[i].rewards.Length > 0)
                reward.gameObject.SetActive(true);
        }
        if (ach.Earned)
            gameObject.GetComponent<Image>().color = new Color32(0, 255, 44, 255);
    }

    public void RefreshProgress()
    {
        achievementName.text = achievement.GetAchievementName();
        achievementProgress.text = $"{achievement.CurrentAmount}/{achievement.tiers[achievement.Tier].amount}";
        for (int i = 0; i < achievement.tiers.Length; i++)
        {
            TextMeshProUGUI reward = rewardList.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>();
            if (achievement.Tier <= i && achievement.CurrentAmount != achievement.tiers[achievement.Tier].amount)
                reward.color = Color.red;
            else
                reward.color = Color.green;
        }
        if (achievement.Earned)
            gameObject.GetComponent<Image>().color = new Color32(0, 255, 44, 255);
    }
}
