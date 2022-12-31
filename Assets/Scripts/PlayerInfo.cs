using System.Collections;
using System.Collections.Generic;
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

        statsButtons[0].onClick.AddListener(delegate { Player.data.AddStrength(true, 1); RefreshStats(); });
        statsButtons[1].onClick.AddListener(delegate { Player.data.AddIntelligence(true, 1); RefreshStats(); });
        statsButtons[2].onClick.AddListener(delegate { Player.data.AddDexterity(true, 1); RefreshStats(); });
    }

    private void CheckPoints()
    {
        if (Player.data.remainingPoints > 0)
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
        switch (Player.data.playerClass)
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
        playerName.text = Player.data.nickname;
        lvl.text = $"{Player.data.level}";
        damage.text = $"{Player.data.damage.GetValue()}";
        defense.text = $"{Player.data.defense.GetValue()}";
        health.text = $"{Player.data.health.GetValue()}";
        critDamage.text = $"{Player.data.criticalDamage.GetValue()}%";
        critRate.text = $"{Player.data.criticalRate.GetValue()}%";
        maxCritRate.text = $"{Player.data.maxCriticalRate.GetValue()}%";
        accuracy.text = $"{Player.data.accuracy.GetValue()}";
        hpSteal.text = $"{Player.data.hpSteal.GetValue()}%";
        hpStealLimit.text = $"{Player.data.hpStealLimit.GetValue()}";
        strength.text = $"{Player.data.strength.GetValue()}";
        intelligence.text = $"{Player.data.intelligence.GetValue()}";
        dexterity.text = $"{Player.data.dexterity.GetValue()}";
        expBonus.text = $"{Player.data.expBonus.GetValue()}";
        goldBonus.text = $"{Player.data.goldBonus.GetValue()}";
        twoslotDropBonus.text = $"{Player.data.twoSlotDropBonus.GetValue()}";
        killedMonsters.text = $"{Player.data.killedMonsters}";
        completedQuests.text = $"{Player.data.completedQuests}";
        earnedAchievements.text = $"{Player.data.earnedAchievements}";
        CheckPoints();
    }
}
