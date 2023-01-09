using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI defense;
    [SerializeField] private TextMeshProUGUI criticalDamage;
    [SerializeField] private TextMeshProUGUI criticalChance;

    [SerializeField] private EquipmentSlot[] slots;

    public void SetUp(EnemyData data)
    {
        enemyName.text = data.enemyName;
        level.text = data.level.ToString();
        damage.text = $"{data.damage}";
        defense.text = data.defense.ToString();
        criticalChance.text = data.criticalChance.ToString();
        criticalDamage.text = data.criticalDamage.ToString();
    }
}
