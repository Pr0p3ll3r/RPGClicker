using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scroll", menuName = "RPG/Items/Scroll")]
public class Scroll : Item
{
    [Header("Scroll")]
    public StatBonus scrollStat;
    public bool inHelmet;
    public bool inChestplate;
    public bool inGloves;
    public bool inBoots;
    public bool inWeapon;
    public bool inPet;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }
}
