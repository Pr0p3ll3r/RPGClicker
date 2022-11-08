using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scroll", menuName = "RPG/Items/Scroll")]
public class Scroll : Item
{
    [Header("Scroll")]
    public StatBonus scrollStat;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }
}
