using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI defense;
    [SerializeField] private TextMeshProUGUI strength;
    [SerializeField] private TextMeshProUGUI stamina;
    [SerializeField] private TextMeshProUGUI vitality;
    [SerializeField] private TextMeshProUGUI agility;
    [SerializeField] private TextMeshProUGUI luck;
    [SerializeField] private TextMeshProUGUI criticalDamage;
    [SerializeField] private TextMeshProUGUI criticalChance;

    [SerializeField] private EquipmentSlotUI[] slots;

    public void SetUp(Character character)
    {
        characterName.text = character.characterName;
        level.text = character.level.ToString();
        damage.text = $"{character.damage.GetValue()}";
        defense.text = character.defense.GetValue().ToString();
        strength.text = character.strength.GetValue().ToString();
        agility.text = character.agility.GetValue().ToString();
        vitality.text = character.vitality.GetValue().ToString();
        luck.text = character.luck.GetValue().ToString();
        stamina.text = character.stamina.GetValue().ToString();
        criticalChance.text = character.criticalChance.GetValue().ToString();
        criticalDamage.text = character.criticalDamage.GetValue().ToString();
        UpdateHealthBar(character);
        RefreshEquipment(character);
    }

    void RefreshEquipment(Character character)
    {
        for (int i = 0; i < character.equipment.Length; i++)
        {
            if (character.equipment[i] != null)
                slots[i].AddEquipment(character.equipment[i]);
            else
                slots[i].ClearSlot();
        }
    }

    public void UpdateHealthBar(Character character)
    {
        float ratio = (float)character.currentHealth / (float)character.maxHealth;
        healthBar.fillAmount = ratio;
        health.text = $"{character.currentHealth}/{character.maxHealth}";
    }
}
