using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Enemy", menuName = "RPG/Character/Enemy")]
public class EnemyInfo : Character
{
    public Sprite look;
    public bool boss;
    public int rewardGold;
    public int rewardExp;
    public int possibleRarity;
    public List<LootItem> loot = new List<LootItem>();
    public List<EquipmentBase> equipmentBases;
    public Location bossLocation;

    public void LoadEquipment()
    {
        Initialize();
        foreach (EquipmentBase eqBase in equipmentBases)
        {
            Equipment eq = Loot.BossItem(eqBase);
            Equip(eq, (int)eq.equipSlot);
        }
    }

    public void ResetEquipment()
    {
        foreach (EquipmentBase eq in equipmentBases)
        {
            Unequip(eq, (int)eq.equipSlot);
        }
    }

    void Equip(Equipment item, int numSlot)
    {
        if (level < item.lvlRequired) return;

        equipment[numSlot] = item;
        damage.AddModifier(item.damage);
        defense.AddModifier(item.defense);
        strength.AddModifier(item.strength);
        criticalDamage.AddModifier(item.criticalDamage);
        vitality.AddModifier(item.vitality);
        criticalChance.AddModifier(item.criticalChance);
    }

    void Unequip(EquipmentBase item, int numSlot)
    {
        equipment[numSlot] = null;
        damage.RemoveModifier(item.damage);
        defense.RemoveModifier(item.defense);
        strength.RemoveModifier(item.strength);
        criticalDamage.RemoveModifier(item.criticalDamage);
        vitality.RemoveModifier(item.vitality);
        criticalChance.RemoveModifier(item.criticalChance);
    }
}
