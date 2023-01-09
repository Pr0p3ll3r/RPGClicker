using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI lvl;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI defense;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI critDamage;
    [SerializeField] private TextMeshProUGUI critRate;
    [SerializeField] private TextMeshProUGUI maxCritRate;
    [SerializeField] private TextMeshProUGUI accuracy;
    [SerializeField] private TextMeshProUGUI hpSteal;
    [SerializeField] private TextMeshProUGUI hpStealLimit;
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI intelligence;
    [SerializeField] private TextMeshProUGUI dexterity;
    [SerializeField] private Button[] statsButtons;
    [SerializeField] private TextMeshProUGUI strengthBonuses;
    [SerializeField] private TextMeshProUGUI intelligenceBonuses;
    [SerializeField] private TextMeshProUGUI dexterityBonuses;

    [Header("More Stats")]
    [SerializeField] private TextMeshProUGUI expBonus;
    [SerializeField] private TextMeshProUGUI goldBonus;
    [SerializeField] private TextMeshProUGUI twoslotDropBonus;
    [SerializeField] private TextMeshProUGUI killedMonsters;
    [SerializeField] private TextMeshProUGUI completedQuests;
    [SerializeField] private TextMeshProUGUI earnedAchievements;

    private Player Player => Player.Instance;

    private void Start()
    {
        RefreshStats();

        statsButtons[0].onClick.AddListener(delegate { Player.Data.AddStrength(true, 1); RefreshStats(); });
        statsButtons[1].onClick.AddListener(delegate { Player.Data.AddIntelligence(true, 1); RefreshStats(); });
        statsButtons[2].onClick.AddListener(delegate { Player.Data.AddDexterity(true, 1); RefreshStats(); });
    }

    private void CheckPoints()
    {
        if (Player.Data.remainingPoints > 0)
        {
            foreach (Button b in statsButtons)
            {
                b.interactable = true;
            }
        }
        else
        {
            foreach (Button b in statsButtons)
            {
                b.interactable = false;
            }
        }
    }

    public void SetStatsDescription()
    {
        switch (Player.Data.playerClass)
        {
            case PlayerClass.Warrior:
                strengthBonuses.text = "+1 DMG";
                intelligenceBonuses.text = "+2 HP";
                dexterityBonuses.text = "+1 DEF";
                break;
            case PlayerClass.Blader:
                strengthBonuses.text = "+1 DEF";
                intelligenceBonuses.text = "+2 HP";
                dexterityBonuses.text = "+1 DMG";           
                break;
            case PlayerClass.Archer:
                strengthBonuses.text = "+1 DEF";
                intelligenceBonuses.text = "+2 HP";
                dexterityBonuses.text = "+1 DMG";
                break;
            case PlayerClass.Wizard:
                strengthBonuses.text = "+2 HP";
                intelligenceBonuses.text = "+1 DMG";
                dexterityBonuses.text = "+1 DEF";
                break;
            case PlayerClass.Shielder:
                strengthBonuses.text = "+1 DMG";
                intelligenceBonuses.text = "+2 HP";
                dexterityBonuses.text = "+1 DEF";
                break;
        }
    }

    public void RefreshStats()
    {
        playerName.text = Player.Data.nickname;
        lvl.text = $"{Player.Data.level}";
        damage.text = $"{Player.Data.damage.GetValue()}";
        defense.text = $"{Player.Data.defense.GetValue()}";
        health.text = $"{Player.Data.health.GetValue()}";
        critDamage.text = $"{Player.Data.criticalDamage.GetValue()}%";
        critRate.text = $"{Player.Data.criticalRate.GetValue()}%";
        maxCritRate.text = $"{Player.Data.maxCriticalRate.GetValue()}%";
        accuracy.text = $"{Player.Data.accuracy.GetValue()}";
        hpSteal.text = $"{Player.Data.hpSteal.GetValue()}%";
        hpStealLimit.text = $"{Player.Data.hpStealLimit.GetValue()}";
        strength.text = $"{Player.Data.strength.GetValue()}";
        intelligence.text = $"{Player.Data.intelligence.GetValue()}";
        dexterity.text = $"{Player.Data.dexterity.GetValue()}";
        expBonus.text = $"{Player.Data.expBonus.GetValue()}%";
        goldBonus.text = $"{Player.Data.goldBonus.GetValue()}%";
        twoslotDropBonus.text = $"{Player.Data.twoSlotDropBonus.GetValue()}%";
        killedMonsters.text = $"{Player.Data.killedMonsters}";
        completedQuests.text = $"{Player.Data.completedQuests}";
        earnedAchievements.text = $"{Player.Data.earnedAchievements}";
        CheckPoints();
    }
}
