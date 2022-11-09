using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] private Button statsButton;
    [SerializeField] private Button moreStatsButton;
    [SerializeField] private Button petsButton;
    [SerializeField] private Button equipmentButton;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        ClosePanels();
        statsPanel.SetActive(true);
        revivePanel.SetActive(false);

        statsButton.onClick.AddListener(delegate { ClosePanels(); statsPanel.SetActive(true); });
        moreStatsButton.onClick.AddListener(delegate { ClosePanels(); moreStatsPanel.SetActive(true); });
        petsButton.onClick.AddListener(delegate { ClosePanels(); petsPanel.SetActive(true); });
        equipmentButton.onClick.AddListener(delegate { ClosePanels(); equipmentPanel.SetActive(true); });
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

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    public void UpdateLevel(int level, int exp, int requireExp)
    {
        if(level == 100)
        {
            expText.text = $"MAXED";
            expBar.fillAmount = 1;
        }
        else
        {
            expText.text = $"{exp}/{requireExp}";
            expBar.fillAmount = (float)exp / requireExp;
        }
        levelText.text = $"{level} ";
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
            player.Revive();
        }           
    }

    private void ClosePanels()
    {
        statsPanel.SetActive(false);
        moreStatsPanel.SetActive(false);
        petsPanel.SetActive(false);
        equipmentPanel.SetActive(false);
    }
}