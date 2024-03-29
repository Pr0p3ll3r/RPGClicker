﻿using System.Collections.Generic;
using UnityEngine;

public enum Stats
{
    Damage,
    Defense,
    Health,
    CriticalDamage,
    CriticalRatePercent,
    MaxCriticalRatePercent,
    HPStealPercent,
    HPStealLimit,
    GoldBonusPercent,
    TwoSlotItemDropPercent,
    ExpBonusPercent,
    Strength,
    Intelligence,
    Dexterity,
    MaxPets
}

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int baseValue;

    [SerializeField]
    private List<int> modifiers;

    public Stat(int _baseValue)
    {
        baseValue = _baseValue;
        modifiers = new List<int>();
    }

    public int GetValue()
    {
        int finalValue = baseValue;
        for (int i = 0; i < modifiers.Count; i++)
        {
            finalValue += modifiers[i];
        }
        return finalValue;
    }

    public void AddModifier(int modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        modifiers.Remove(modifier);
    }
}
