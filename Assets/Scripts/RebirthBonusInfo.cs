using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebirthBonusInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bonusName;
    [SerializeField] private Image bonusIcon;
    [SerializeField] private TextMeshProUGUI currentBonus;
    [SerializeField] private TextMeshProUGUI nextBonus;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private RebirthSystem rebirthSystem;

    private PlayerData Data => Player.Instance.Data;

    public void SetUp(RebirthBonus bonus)
    {
        upgradeButton.onClick.RemoveAllListeners();
        bonusIcon.sprite = bonus.icon;
        int level = bonus.CurrentLevel;
        bonusName.text = $"{bonus.bonusName} ({level}/{bonus.levels.Length - 1})";     
        currentBonus.text = $"Current: +{bonus.CurrentValue}";
        if(level == bonus.levels.Length - 1)
            nextBonus.text = "MAXED";
        else
            nextBonus.text = $"Next: +{bonus.levels[level + 1].value}";
        upgradeButton.onClick.AddListener(delegate { Upgrade(bonus); });

        if (level == bonus.levels.Length - 1 || Data.rebirthRemainingPoints < bonus.levels[level + 1].cost)
            upgradeButton.interactable = false;
        else
            upgradeButton.interactable = true;

        gameObject.SetActive(true);
    }

    private void Upgrade(RebirthBonus bonus)
    {
        Data.AddStat(bonus.stat, bonus.levels[bonus.CurrentLevel + 1].value);
        bonus.CurrentValue += bonus.levels[bonus.CurrentLevel + 1].value;
        bonus.CurrentLevel++;
        Data.rebirthRemainingPoints--;
        SoundManager.Instance.PlayOneShot("Click");
        SetUp(bonus);
        rebirthSystem.RefreshBonus(bonus);
    }
}
