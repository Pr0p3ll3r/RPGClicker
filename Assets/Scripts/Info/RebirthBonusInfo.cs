using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebirthBonusInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bonusName;
    [SerializeField] private Image bonusIcon;
    [SerializeField] private TextMeshProUGUI currentBonus;
    [SerializeField] private TextMeshProUGUI nextBonus;
    [SerializeField] private TextMeshProUGUI bonusCost;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private RebirthSystem rebirthSystem;

    private PlayerData Data => Player.Instance.Data;

    public void SetUp(RebirthBonus bonus)
    {
        upgradeButton.onClick.RemoveAllListeners();
        bonusIcon.sprite = bonus.icon;
        int level = bonus.CurrentLevel;
        bonusName.text = $"{bonus.bonusName} ({level}/{bonus.levels.Length - 1})";
        if (bonus.stat.ToString().Contains("Percent"))
            currentBonus.text = $"Current: +{bonus.CurrentValue}% {Utils.GetShortName(bonus.stat)}";
        else
            currentBonus.text = $"Current: +{bonus.CurrentValue} {Utils.GetShortName(bonus.stat)}";
        if(level == bonus.levels.Length - 1) 
        {
            nextBonus.text = "Next: MAXED";
            bonusCost.text = "Cost: X";
        }
        else
        {
            if(bonus.stat.ToString().Contains("Percent"))
                nextBonus.text = $"Next: +{bonus.levels[level + 1].value}%";
            else
                nextBonus.text = $"Next: +{bonus.levels[level + 1].value}";
            bonusCost.text = $"Cost: {bonus.levels[level + 1].cost} points";
        }          
        upgradeButton.onClick.AddListener(delegate { Upgrade(bonus); });

        if (level == bonus.levels.Length - 1 || Data.rebirthRemainingPoints < bonus.levels[level + 1].cost)
            upgradeButton.interactable = false;
        else
            upgradeButton.interactable = true;

        gameObject.SetActive(true);
    }

    private void Upgrade(RebirthBonus bonus)
    {
        SoundManager.Instance.PlayOneShot("Click");
        Data.AddStat(bonus.stat, bonus.levels[bonus.CurrentLevel + 1].value);
        bonus.CurrentValue += bonus.levels[bonus.CurrentLevel + 1].value;
        Data.rebirthRemainingPoints -= bonus.levels[bonus.CurrentLevel + 1].cost;
        bonus.CurrentLevel++;
        SetUp(bonus);
        rebirthSystem.RefreshBonus(bonus);
    }
}
