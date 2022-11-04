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
    [SerializeField] private TextMeshProUGUI deadText;

    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject moreStatsPanel;
    [SerializeField] private GameObject petsPanel;
    [SerializeField] private GameObject equipmentPanel;
    [SerializeField] private Button statsButton;
    [SerializeField] private Button moreStatsButton;
    [SerializeField] private Button petsButton;
    [SerializeField] private Button equipmentButton;

    private void Start()
    {
        ClosePanels();
        statsPanel.SetActive(true);

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
        float ratio = (float)currentHealth / maxHealth;
        healthBar.fillAmount = ratio;
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    public void UpdateLevel(int level, int exp, int requireExp)
    {
        levelText.text = $"{level} ";
        expText.text = $"{exp}/{requireExp}";
        float percentage = (float)exp / requireExp;
        expBar.fillAmount = percentage;
    }

    public void ShowVignette()
    {
        StartCoroutine(FadeToZeroAlpha());
    }

    public void ShowDeadText()
    {
        vignette.GetComponent<CanvasGroup>().alpha = 1f;
        deadText.text = "YOU ARE DEAD!";
    }

    private void ClosePanels()
    {
        statsPanel.SetActive(false);
        moreStatsPanel.SetActive(false);
        petsPanel.SetActive(false);
        equipmentPanel.SetActive(false);
    }
}