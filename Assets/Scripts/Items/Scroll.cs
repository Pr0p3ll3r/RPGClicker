using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scroll", menuName = "RPG/Items/Scroll")]
public class Scroll : Item
{
    [Header("Scroll")]
    public Stats stat;
    public int value;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }
}
