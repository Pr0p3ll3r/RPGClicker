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
    [SerializeField] private GameObject playerUI;
    [SerializeField] private Button[] statsButtons;

    private Player player;

    private void Start()
    {
        playerUI.SetActive(false);
        player = GetComponent<Player>();
        RefreshStats();
    }

    void CheckPoints()
    {
        if (player.data.remainPoints > 0)
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
        lvl.text = $"{player.data.level}";
        damage.text = $"{player.data.damage.GetValue()}";
        defense.text = $"{player.data.defense.GetValue()}";
        health.text = $"{player.data.maxHealth.GetValue()}";
        critDamage.text = $"{player.data.criticalDamage.GetValue()}%";
        critRate.text = $"{player.data.criticalRate.GetValue()}%";
        strength.text = $"{player.data.strength.GetValue()}";
        intelligence.text = $"{player.data.intelligence.GetValue()}";
        dexterity.text = $"{player.data.dexterity.GetValue()}";
    }
}
