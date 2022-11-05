using System.Collections;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    private static int BASE_REQUIRE_EXP = 60;

    private int requireExp = 60;

    private PlayerUI hud;
    private PlayerInfo playerInfo;
    private PlayerData data;

    private void Start()
    {
        hud = GetComponent<PlayerUI>();
        playerInfo = GetComponent<PlayerInfo>();
        data = Player.Instance.data;
        requireExp = BASE_REQUIRE_EXP * data.level;
        hud.UpdateLevel(data.level, data.exp, requireExp);
    }

    public void GetExp(int amount)
    {
        data.exp += amount;

        CheckLevelUp();
    }

    void CheckLevelUp()
    {
        if (data.exp < requireExp) return;

        data.exp -= requireExp;
        data.level++;
        data.remainingPoints++;
        GameManager.Instance.ShowText("Level Up!", Color.green);
        requireExp *= 2;
        playerInfo.RefreshStats();
        hud.UpdateLevel(data.level, data.exp, requireExp);
    }
}
