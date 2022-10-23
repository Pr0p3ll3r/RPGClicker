using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static Database Instance { get; private set; }
    public ItemDatabase itemDatabase;

    [SerializeField] private EquipmentUpgrade[] upgradeInfos;
    public static EquipmentUpgrade[] allUpgradeInfos { get; private set; }

    [SerializeField] private EnemyInfo[] towerEnemies;
    public static EnemyInfo[] TowerEnemies { get; private set; }

    [SerializeField] private Location[] locations;
    public static Location[] Locations { get; private set; }

    [SerializeField] private Location[] bosses;
    public static Location[] Bosses { get; private set; }

    private void Awake()
    {
        Instance = this;
        allUpgradeInfos = upgradeInfos;
        TowerEnemies = towerEnemies;
        Locations = locations;
        Bosses = bosses;
        allUpgradeInfos = upgradeInfos;
    }

    //public static EquipmentUpgrade FindItem(EquipmentData eq)
    //{
    //    foreach(EquipmentUpgrade upgrade in allUpgradeInfos)
    //    {
    //        if(eq.itemName.Contains(upgrade.equipmentSet))
    //        {
    //            return upgrade;
    //        }
    //    }
    //    return null;
    //}
}
