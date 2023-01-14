using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private Transform achievementList;
    [SerializeField] private GameObject achievementPrefab;

    private void Start()
    {
        foreach(var ach in Database.data.achievements) 
        {
            GameObject achGO = Instantiate(achievementPrefab, achievementList);
            achGO.GetComponent<AchievementListItem>().SetAchievement(ach);
        }
    }

    public void AchievementProgress(EnemyData enemy)
    {
        Achievement achievement = enemy.achievement;

        if (achievement == null)
            return;

        if (achievement.Earned || enemy.achievement.tiers[enemy.achievement.Tier].goal != enemy)
            return;

        achievement.IncreaseCurrentAmount();
        achievementList.GetChild(achievement.ID).GetComponent<AchievementListItem>().RefreshProgress();
    }
}
