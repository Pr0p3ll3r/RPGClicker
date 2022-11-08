using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatBonus", menuName = "RPG/StatBonus")]
public class StatBonus : ScriptableObject
{
    public Stats stat;
    public int[] values;
}
