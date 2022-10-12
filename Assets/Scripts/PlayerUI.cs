using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [HideInInspector] public PlayerInfo info;
    [Header("Main Stats")]
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI defense;
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI stamina;
    [SerializeField] private TextMeshProUGUI agility;
    [SerializeField] private TextMeshProUGUI vitality;
    [SerializeField] private TextMeshProUGUI luck;
    [SerializeField] private TextMeshProUGUI criticalDamage;
    [SerializeField] private TextMeshProUGUI criticalChance;
    [SerializeField] private Button[] statsButtons;

    [Header("Other Stats")]
    [SerializeField] private TextMeshProUGUI killedMonsters;

    private void Start()
    {
        statsButtons[0].onClick.AddListener(delegate { AddStrength(); });
        statsButtons[1].onClick.AddListener(delegate { AddStamina(); });
        statsButtons[2].onClick.AddListener(delegate { AddVitality(); });
        statsButtons[3].onClick.AddListener(delegate { AddAgility(); });
        statsButtons[4].onClick.AddListener(delegate { AddLuck(); });
    }

    void AddStrength()
    {
        info.AddStrength();
        RefreshStats();
    }

    void AddVitality()
    {
        info.AddVitality();
        info.Initialize();
        RefreshStats();
    }

    void AddLuck()
    {
        info.AddLuck();
        RefreshStats();
    }

    void AddAgility()
    {
        info.AddAgility();
        RefreshStats();
    }

    void AddStamina()
    {
        info.AddStamina();
        RefreshStats();
    }

    void CheckPoints()
    {
        if (info.remainPoints > 0)
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
        strength.text = info.strength.GetValue().ToString();
        stamina.text = info.stamina.GetValue().ToString();
        vitality.text = info.vitality.GetValue().ToString();
        agility.text = info.agility.GetValue().ToString();
        luck.text = info.luck.GetValue().ToString();

        damage.text = info.damage.GetValue().ToString();
        defense.text = info.defense.GetValue().ToString();
        criticalDamage.text = info.criticalDamage.GetValue().ToString() + "%";
        criticalChance.text = info.criticalChance.GetValue().ToString() + "%";

        CheckPoints();
    }

    public void RefreshOtherStats()
    {
        killedMonsters.text = info.killedMonsters.ToString();
    }
}
