using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Database
{
    public static DatabaseSO data;

    public static EquipmentUpgrade FindItem(Equipment eq)
    {
        foreach (EquipmentUpgrade upgrade in data.allUpgradeInfos)
        {
            if (eq.itemName.Contains(upgrade.equipmentSet))
            {
                return upgrade;
            }
        }
        return null;
    }
}
