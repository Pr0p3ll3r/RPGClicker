using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebirthSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rebirthLevel;
    [SerializeField] private TextMeshProUGUI rebirthPoints;
    [SerializeField] private Transform rebirthBonusesList;
    [SerializeField] private GameObject rebirthBonusPrefab;
    [SerializeField] private Button rebirthButton;
    [SerializeField] private RebirthBonusInfo rebirthBonusInfo;
    [SerializeField] private GameObject rebirthPanel;

    public static event Action OnRebirth;

    private int maxLevel = 100;
    private PlayerData Data => Player.Instance.Data;

    private void Start()
    {
        rebirthBonusInfo.gameObject.SetActive(false);
        rebirthButton.onClick.AddListener(delegate { Rebirth(); });

        foreach (var rb in Database.data.rebirthBonuses)
        {
            GameObject bonus = Instantiate(rebirthBonusPrefab, rebirthBonusesList);
            bonus.transform.Find("IconBG/Icon").GetComponent<Image>().sprite = rb.icon;
            bonus.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = $"{rb.CurrentLevel}/{rb.levels.Length - 1}";
            bonus.GetComponent<Button>().onClick.AddListener(delegate { rebirthBonusInfo.SetUp(rb); });
        }
    }

    public void ShowRebirthPanel()
    {
        rebirthPanel.SetActive(true);
        if (Data.level == LevelSystem.MaxLevel && Data.rebirthLevel != maxLevel)
            rebirthButton.interactable = true;
        else
            rebirthButton.interactable = false;
        rebirthLevel.text = $"Level: {Data.rebirthLevel}";
        rebirthPoints.text = $"Points: {Data.rebirthRemainingPoints}";
    }

    public void RefreshBonus(RebirthBonus bonus)
    {
        rebirthBonusesList.GetChild(bonus.ID).transform.Find("Level").GetComponent<TextMeshProUGUI>().text = $"{bonus.CurrentLevel}/{bonus.levels.Length - 1}";
        rebirthLevel.text = $"Level: {Data.rebirthLevel}";
        rebirthPoints.text = $"Points: {Data.rebirthRemainingPoints}";
    }

    private void Rebirth()
    {
        Data.rebirthLevel++;
        Data.rebirthRemainingPoints++;
        rebirthLevel.text = $"Level: {Data.rebirthLevel}";
        rebirthPoints.text = $"Points: {Data.rebirthRemainingPoints}";
        GameManager.Instance.ShowText("Rebirth!", Color.yellow);
        rebirthBonusInfo.gameObject.SetActive(false);
        rebirthPanel.SetActive(false);
        OnRebirth?.Invoke();
    }
}
