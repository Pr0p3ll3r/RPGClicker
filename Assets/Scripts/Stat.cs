﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int baseValue;

    [NonSerialized] private List<int> modifiers = new List<int>();

    public int GetValue()
    {
        int finalValue = baseValue;
        for(int i=0;i<modifiers.Count;i++)
        {
            finalValue += modifiers[i];
        }
        return finalValue;
    }

    public void AddPoint(int amount)
    {
        baseValue += amount;
    }

    public void AddModifier(int modifier)
    {
        if (modifier != 0)
            modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
            modifiers.Remove(modifier);
    }
}
