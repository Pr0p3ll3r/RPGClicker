using UnityEngine;

[CreateAssetMenu(fileName = "StatBonus", menuName = "RPG/StatBonus")]
public class StatBonus : ScriptableObject
{
    public int ID;
    public Stats stat;
    /// <summary>
    /// <para>In Scrolls: 0 - Value for equipment, 1 - Value for pet</para>
    /// <para>In Rarity: 0 - Common value, 1 - Uncommon value, 2 - Epic value, 3 - Legendary value</para>
    /// </summary>
    public int[] values;
}
