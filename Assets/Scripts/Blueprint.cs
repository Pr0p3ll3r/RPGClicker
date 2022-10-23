using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blueprint", menuName = "RPG/Items/Blueprint")]
public class Blueprint : Item
{
    public Equipment resultItem;
    public List<NeedItem> needItems;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }
}