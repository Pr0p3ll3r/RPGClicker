using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatBonus", menuName = "RPG/StatBonus")]
public class StatBonus : ScriptableObject
{
    public int ID;
    public Stats stat;
    /// <summary>
    /// <para>In Scrolls: 0 - Normal value, 1 - Value for 2H weapon , 2 - Value for pet</para>
    /// <para>In Rarity: 0 - Uncommon value, 2 - Epic value, 3 - Legendary value</para>
    /// </summary>
    public int[] values;
}
