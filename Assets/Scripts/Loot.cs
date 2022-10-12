using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Loot 
{
    public static Item Drop(LootItem lootBasic, float chance, int possibleRarity)
    {
        if (!ChanceToDrop(chance)) return null;

        if (lootBasic.item.GetType() == typeof(EquipmentBase))
        {
            EquipmentRarity rarity;
            rarity = Rarity(possibleRarity);
            EquipmentBase eqBase = ScriptableObject.CreateInstance<EquipmentBase>();
            eqBase.NewAsset((EquipmentBase)lootBasic.item);
            eqBase.rarity = rarity;
            switch (rarity)
            {
                case EquipmentRarity.Common:
                    eqBase.damage = Random.Range(eqBase.damageMinCommon, eqBase.damageMaxCommon + 1);
                    eqBase.defense = Random.Range(eqBase.defenseMinCommon, eqBase.defenseMaxCommon + 1);
                    eqBase.strength = Random.Range(eqBase.strengthMinCommon, eqBase.strengthMaxCommon + 1);
                    eqBase.criticalDamage = Random.Range(eqBase.criticalDamageMinCommon, eqBase.criticalDamageMaxCommon + 1);
                    eqBase.vitality = Random.Range(eqBase.vitalityMinCommon, eqBase.vitalityMaxCommon + 1);
                    eqBase.criticalChance = Random.Range(eqBase.criticalChanceMinCommon, eqBase.criticalChanceMaxCommon + 1);
                    break;
                case EquipmentRarity.Epic:
                    eqBase.damage = Random.Range(eqBase.damageMinEpic, eqBase.damageMaxEpic + 1);
                    eqBase.defense = Random.Range(eqBase.defenseMinEpic, eqBase.defenseMaxEpic + 1);
                    eqBase.strength = Random.Range(eqBase.strengthMinEpic, eqBase.strengthMaxEpic + 1);
                    eqBase.criticalDamage = Random.Range(eqBase.criticalDamageMinEpic, eqBase.criticalDamageMaxEpic + 1);
                    eqBase.vitality = Random.Range(eqBase.vitalityMinEpic, eqBase.vitalityMaxEpic + 1);
                    eqBase.criticalChance = Random.Range(eqBase.criticalChanceMinEpic, eqBase.criticalChanceMaxEpic + 1);
                    break;
                case EquipmentRarity.Rare:
                    eqBase.damage = Random.Range(eqBase.damageMinRare, eqBase.damageMaxRare + 1);
                    eqBase.defense = Random.Range(eqBase.defenseMinRare, eqBase.defenseMaxRare + 1);
                    eqBase.strength = Random.Range(eqBase.strengthMinRare, eqBase.strengthMaxRare + 1);
                    eqBase.criticalDamage = Random.Range(eqBase.criticalDamageMinRare, eqBase.criticalDamageMaxRare + 1);
                    eqBase.vitality = Random.Range(eqBase.vitalityMinRare, eqBase.vitalityMaxRare + 1);
                    eqBase.criticalChance = Random.Range(eqBase.criticalChanceMinRare, eqBase.criticalChanceMaxRare + 1);
                    break;
                case EquipmentRarity.Legendary:
                    eqBase.damage = Random.Range(eqBase.damageMinLegendary, eqBase.damageMaxLegendary + 1);
                    eqBase.defense = Random.Range(eqBase.defenseMinLegendary, eqBase.defenseMaxLegendary + 1);
                    eqBase.strength = Random.Range(eqBase.strengthMinLegendary, eqBase.strengthMaxLegendary + 1);
                    eqBase.criticalDamage = Random.Range(eqBase.criticalDamageMinLegendary, eqBase.criticalDamageMaxLegendary + 1);
                    eqBase.vitality = Random.Range(eqBase.vitalityMinLegendary, eqBase.vitalityMaxLegendary + 1);
                    eqBase.criticalChance = Random.Range(eqBase.criticalChanceMinLegendary, eqBase.criticalChanceMaxLegendary + 1);
                    break;
            }

            Equipment eq = ScriptableObject.CreateInstance<Equipment>();
            eq.CreateEq(eqBase);
            ScriptableObject.Destroy(eqBase);
            eq.cost = CalculateCost(eq);
            return eq;
        }
        else if (lootBasic.item.GetType() == typeof(Blueprint))
        {
            Blueprint blueprint = ScriptableObject.CreateInstance<Blueprint>();
            blueprint.NewAsset((Blueprint)lootBasic.item);
            return blueprint;
        }
        else
        {
            Item item = ScriptableObject.CreateInstance<Item>();
            item.NewAsset(lootBasic.item);
            return item;         
        }
    }

    public static Equipment Craft(EquipmentBase eqBase)
    {
        EquipmentBase equipmentBase = ScriptableObject.CreateInstance<EquipmentBase>();
        equipmentBase.NewAsset(eqBase);
        EquipmentRarity rarity = EquipmentRarity.Legendary;
        equipmentBase.rarity = rarity;
        equipmentBase.damage = Random.Range(equipmentBase.damageMinLegendary, equipmentBase.damageMinLegendary + 1);
        equipmentBase.defense = Random.Range(equipmentBase.defenseMinLegendary, equipmentBase.defenseMaxLegendary + 1);
        equipmentBase.strength = Random.Range(equipmentBase.strengthMinLegendary, equipmentBase.strengthMaxLegendary + 1);
        equipmentBase.criticalDamage = Random.Range(equipmentBase.criticalDamageMinLegendary, equipmentBase.criticalDamageMaxLegendary + 1);
        equipmentBase.vitality = Random.Range(equipmentBase.vitalityMinLegendary, equipmentBase.vitalityMaxLegendary + 1);
        equipmentBase.criticalChance = Random.Range(equipmentBase.criticalChanceMinLegendary, equipmentBase.criticalChanceMaxLegendary + 1);
        Equipment eq = ScriptableObject.CreateInstance<Equipment>();
        eq.CreateEq(equipmentBase);
        ScriptableObject.Destroy(equipmentBase);
        eq.cost = CalculateCost(eq);
        return eq;
    }

    public static Equipment BossItem(EquipmentBase eqBase)
    {
        EquipmentBase equipmentBase = ScriptableObject.CreateInstance<EquipmentBase>();
        equipmentBase.NewAsset(eqBase);
        EquipmentRarity rarity = EquipmentRarity.Legendary;
        equipmentBase.rarity = rarity;
        equipmentBase.damage = equipmentBase.damageMinLegendary;
        equipmentBase.defense = equipmentBase.defenseMaxLegendary;
        equipmentBase.strength = equipmentBase.strengthMaxLegendary;
        equipmentBase.criticalDamage = equipmentBase.criticalDamageMaxLegendary;
        equipmentBase.vitality = equipmentBase.vitalityMaxLegendary;
        equipmentBase.criticalChance = equipmentBase.criticalChanceMaxLegendary;
        Equipment eq = ScriptableObject.CreateInstance<Equipment>();
        eq.CreateEq(equipmentBase);
        ScriptableObject.Destroy(equipmentBase);
        eq.cost = 0;
        return eq;
    }

    static bool ChanceToDrop(float chance)
    {
        int randomNumber = Random.Range(1, 101);
        //Debug.Log(randomNumber);
        if (randomNumber <= chance * 100)
        {
            return true;
        }
        else return false;
    }

    static EquipmentRarity Rarity(int possibleRarity)
    {
        int randomNumber = Random.Range(1, 101);
        switch (possibleRarity)
        { 
            case 0:
                return EquipmentRarity.Common;
            case 1:
                if (randomNumber <= 30)
                {
                    return EquipmentRarity.Rare;
                }
                else
                {
                    return EquipmentRarity.Common;
                }
            case 2:
                if (randomNumber <= 10)
                {
                    return EquipmentRarity.Epic;
                }
                else if (randomNumber <= 30)
                {
                    return EquipmentRarity.Rare;
                }
                else
                {
                    return EquipmentRarity.Common;
                }
            default: 
                return EquipmentRarity.Common;
        }
    }

    static int CalculateCost(Equipment item)
    {
        int cost = item.cost;
        cost += item.defense;
        cost += item.strength;
        cost += item.vitality;
        cost += item.criticalDamage;
        cost += item.criticalChance;
        cost += item.damage;
        return cost;
    }
}
