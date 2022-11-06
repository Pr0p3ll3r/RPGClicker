using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RarityBonus", menuName = "RPG/RarityBonus")]
public class RarityBonus : ScriptableObject
{
    public Stats stat;
    public int uncommonValue;
    public int epicValue;
    public int legendaryValue;
    public int[] values = new int[3];
}
