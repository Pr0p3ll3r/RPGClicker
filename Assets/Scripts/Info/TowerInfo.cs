using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class TowerInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private Image enemyIcon;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI defense;
    [SerializeField] private TextMeshProUGUI criticalDamage;
    [SerializeField] private TextMeshProUGUI criticalChance;
    [SerializeField] private EquipmentSlot[] slots;
    [SerializeField] private ItemInfo selectedItem;

    public void SetUp(EnemyData data)
    {
        if(data == null)
        {
            enemyName.text = "COMPLETED";
            enemyIcon.sprite = Database.data.emptySlot;
            level.text = "X";
            damage.text = "X";
            defense.text = "X";
            criticalChance.text = "X";
            criticalDamage.text = "X";
            health.text = $"X/X";
            healthBar.fillAmount = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].ClearSlot();
            }
        }
        else
        {
            enemyName.text = data.enemyName;
            enemyIcon.sprite = data.icon;
            level.text = data.level.ToString();
            damage.text = $"{data.damage}";
            defense.text = data.defense.ToString();
            criticalChance.text = data.criticalChance.ToString();
            criticalDamage.text = data.criticalDamage.ToString();
            float ratio = (float)data.health / data.health;
            health.text = $"{data.health}/{data.health}";
            healthBar.fillAmount = ratio;
            for (int i = 0; i < data.equipment.Length; i++)
            {
                slots[i].ClearSlot();
                if (data.equipment[i] != null)
                    slots[i].FillSlot(data.equipment[i]);                 
            }
        }
    }
}
