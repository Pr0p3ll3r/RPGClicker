using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static int BASE_REQUIRE_EXP = 10; 
    private int requireExp = 10;
    public static int MaxLevel { get; private set; }

    private PlayerUI hud;
    private PlayerInfo playerInfo;
    private PlayerData Data => Player.Instance.Data;

    private void Start()
    {
        MaxLevel = 100;
        hud = GetComponent<PlayerUI>();
        playerInfo = GetComponent<PlayerInfo>();
        requireExp = BASE_REQUIRE_EXP * Data.level;
        hud.UpdateLevel();
    }

    public void GetExp(int amount)
    {
        if (Data.level == MaxLevel) return;

        Data.exp += amount;
        hud.UpdateLevel();

        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (Data.level == MaxLevel)
        {
            Data.exp = 0;
            hud.UpdateLevel();
            return;
        }

        if (Data.exp < requireExp)
            return;
        
        Data.exp -= requireExp;
        Data.level++;
        Data.remainingPoints++;
        GameManager.Instance.ShowText("Level Up!", Color.green);
        requireExp = BASE_REQUIRE_EXP * Data.level;
        playerInfo.RefreshStats();
        hud.UpdateLevel();
        //Debug.Log("Level Up! Current level: " + data.level);
        CheckLevelUp();
    }
}
