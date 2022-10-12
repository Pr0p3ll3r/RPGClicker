using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blueprint", menuName = "RPG/Item/Blueprint")]
public class Blueprint : Item
{
    public EquipmentBase resultItem;
    public List<NeededItem> neededItems;

    public void NewAsset(Blueprint basic)
    {
        base.NewAsset(basic);
        resultItem = basic.resultItem;
        neededItems = basic.neededItems;
    }
}