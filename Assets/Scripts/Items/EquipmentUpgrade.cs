using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeInfo", menuName = "RPG/UpgradeInfo")]
public class EquipmentUpgrade : ScriptableObject
{
    public string equipmentSet;
    public UpgradeLevel[] upgradeLevels;
}

[System.Serializable]
public class UpgradeLevel
{
    public string level;
    public NeedItem[] neededItems;
}