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

    public void AchievementProgress(Achievement achievement)
    {
        if (achievement == null)
            return;

        if (achievement.Earned)
            return;

        achievement.IncreaseCurrentAmount();
        achievementList.GetChild(achievement.ID).GetComponent<AchievementListItem>().RefreshProgress();
    }
}
