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

    public void AchievementProgress(Achievement[] achievements)
    {
        foreach(Achievement ach in achievements) 
        {
            if (ach.GetEarned())
                return;

            ach.IncreaseCurrentAmount();
            achievementList.GetChild(ach.ID).GetComponent<AchievementListItem>().RefreshProgress();
        }     
    }
}
