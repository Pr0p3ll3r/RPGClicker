using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private float fadeOutTime = 4f;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private GameObject vignette;
    [SerializeField] private GameObject revivePanel;
    [SerializeField] private TextMeshProUGUI reviveTimer;

    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject moreStatsPanel;
    [SerializeField] private GameObject petsPanel;
    [SerializeField] private GameObject equipmentPanel;
    [SerializeField] private GameObject achievementsPanel;
    [SerializeField] private Button statsButton;
    [SerializeField] private Button moreStatsButton;
    [SerializeField] private Button petsButton;
    [SerializeField] private Button equipmentButton;
    [SerializeField] private Button achievementsButton;

    private Player Player => Player.Instance;

    private void Start()
    {
        ClosePanels();
        statsPanel.SetActive(true);
        revivePanel.SetActive(false);

        statsButton.onClick.AddListener(delegate { ClosePanels(); statsPanel.SetActive(true); });
        moreStatsButton.onClick.AddListener(delegate { ClosePanels(); moreStatsPanel.SetActive(true); });
        petsButton.onClick.AddListener(delegate { ClosePanels(); petsPanel.SetActive(true); });
        equipmentButton.onClick.AddListener(delegate { ClosePanels(); equipmentPanel.SetActive(true); });
        achievementsButton.onClick.AddListener(delegate { ClosePanels(); achievementsPanel.SetActive(true); });
    }

    private IEnumerator FadeToZeroAlpha()
    {
        vignette.GetComponent<CanvasGroup>().alpha = 0.5f;

        while (vignette.GetComponent<CanvasGroup>().alpha > 0.0f)
        {
            vignette.GetComponent<CanvasGroup>().alpha -= (Time.deltaTime / fadeOutTime);
            yield return null;
        }
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)Player.Data.currentHealth / Player.Data.health.GetValue();
        healthText.text = $"{Player.Data.currentHealth}/{Player.Data.health.GetValue()}";
    }

    public void UpdateLevel()
    {
        int requireExp = LevelSystem.BASE_REQUIRE_EXP * Player.Data.level;
        if (Player.Data.level == 100)
        {
            expText.text = $"MAXED";
            expBar.fillAmount = 1;
        }
        else
        {
            expText.text = $"{Player.Data.exp}/{requireExp}";
            expBar.fillAmount = (float)Player.Data.exp / requireExp;
        }
        levelText.text = $"{Player.Data.level} ";
    }

    public void ShowVignette()
    {
        StartCoroutine(FadeToZeroAlpha());
    }

    public void ShowRevivePanel()
    {
        revivePanel.SetActive(true);
        StartCoroutine(Revive(10));
    }

    private IEnumerator Revive(int value)
    {
        reviveTimer.text = value.ToString();
        yield return new WaitForSeconds(1f);
        value--;
        reviveTimer.text = value.ToString();
        if(value > 0)
            StartCoroutine(Revive(value));
        else
        {
            revivePanel.SetActive(false);
            Player.Revive();
        }           
    }

    private void ClosePanels()
    {
        statsPanel.SetActive(false);
        moreStatsPanel.SetActive(false);
        petsPanel.SetActive(false);
        equipmentPanel.SetActive(false);
        achievementsPanel.SetActive(false);
    }
}