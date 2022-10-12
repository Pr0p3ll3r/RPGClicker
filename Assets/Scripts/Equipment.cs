using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    public EquipmentRarity rarity;
    public int lvlRequired = 1;
    public int level = 0;

    public int damage;
    public int defense;
    public int strength;
    public int vitality;
    public int criticalDamage;
    public int criticalChance;

    public void NewAsset(Equipment basic)
    {
        base.NewAsset(basic);
        equipSlot = basic.equipSlot;
        rarity = basic.rarity;
        lvlRequired = basic.lvlRequired;
        level = basic.level;
    }

    public void CreateEq(EquipmentBase basic)
    {
        NewAsset(basic);
        damage = basic.damage;
        defense = basic.defense;
        criticalDamage = basic.criticalDamage;
        strength = basic.strength;
        vitality = basic.vitality;
        criticalChance = basic.criticalChance;
    }
}

public enum EquipmentSlot
{
    Helmet,
    Chestplate,
    Leggins,
    Feet,
    Hands,
    Weapon,
    Shield,
    Necklace,
    Ring0,
    Ring1,
    Ring2,
    Ring3
}

public enum EquipmentRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
