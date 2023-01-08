using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private static int BASE_REQUIRE_EXP = 10; 
    private int requireExp = 10;
    private int maxLevel = 100;

    private PlayerUI hud;
    private PlayerInfo playerInfo;
    private PlayerData data => Player.Instance.Data;

    private void Start()
    {
        hud = GetComponent<PlayerUI>();
        playerInfo = GetComponent<PlayerInfo>();
        requireExp = BASE_REQUIRE_EXP * data.level;
        hud.UpdateLevel(data.level, data.exp, requireExp);
    }

    public void GetExp(int amount)
    {
        if (data.level == maxLevel) return;

        data.exp += amount;
        hud.UpdateLevel(data.level, data.exp, requireExp);

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (data.level == maxLevel)
        {
            data.exp = 0;
            hud.UpdateLevel(data.level, data.exp, requireExp);
            return;
        }

        if (data.exp < requireExp)
            return;
        
        data.exp -= requireExp;
        data.level++;
        data.remainingPoints++;
        GameManager.Instance.ShowText("Level Up!", Color.green);
        requireExp = BASE_REQUIRE_EXP * data.level;
        playerInfo.RefreshStats();
        hud.UpdateLevel(data.level, data.exp, requireExp);
        //Debug.Log("Level Up! Current level: " + data.level);
        CheckLevelUp();
    }
}
