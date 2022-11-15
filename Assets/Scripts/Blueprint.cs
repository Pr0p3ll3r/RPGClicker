using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blueprint", menuName = "RPG/Items/Blueprint")]
public class Blueprint : Item
{
    public Equipment resultItem;
    public List<RequiredItem> requiredItems;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }
}