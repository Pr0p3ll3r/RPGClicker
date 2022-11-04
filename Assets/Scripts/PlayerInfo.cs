using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lvl;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI defense;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI critDamage;
    [SerializeField] private TextMeshProUGUI critRate;
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI intelligence;
    [SerializeField] private TextMeshProUGUI dexterity;
    [SerializeField] private Button[] statsButtons;

    private Player Player => Player.Instance;

    private void Start()
    {
        RefreshStats();

        statsButtons[0].onClick.AddListener(delegate { Player.data.AddStrength(); RefreshStats(); });
        statsButtons[1].onClick.AddListener(delegate { Player.data.AddIntelligence(); RefreshStats(); });
        statsButtons[2].onClick.AddListener(delegate { Player.data.AddDexterity(); RefreshStats(); });
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

    public void RefreshStats()
    {
        lvl.text = $"{Player.data.level}";
        damage.text = $"{Player.data.damage.GetValue()}";
        defense.text = $"{Player.data.defense.GetValue()}";
        health.text = $"{Player.data.maxHealth.GetValue()}";
        critDamage.text = $"{Player.data.criticalDamage.GetValue()}%";
        critRate.text = $"{Player.data.criticalRate.GetValue()}%";
        strength.text = $"{Player.data.strength.GetValue()}";
        intelligence.text = $"{Player.data.intelligence.GetValue()}";
        dexterity.text = $"{Player.data.dexterity.GetValue()}";
        CheckPoints();
    }
}
