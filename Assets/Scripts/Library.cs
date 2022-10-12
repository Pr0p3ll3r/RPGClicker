using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Library : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] towerEnemies;
    public static EnemyInfo[] TowerEnemies { get; private set; }

    [SerializeField] private Location[] locations;
    public static Location[] Locations { get; private set; }

    [SerializeField] private Location[] bosses;
    public static Location[] Bosses { get; private set; }

    [SerializeField] private Item[] items;
    public static Item[] Items { get; private set; }

    [SerializeField] private EquipmentUpgrade[] upgradeInfos;
    public static EquipmentUpgrade[] allUpgradeInfos { get; private set; }

    public static Dictionary<string, Sprite> icons = new Dictionary<string, Sprite>();

    private void Awake()
    { 
        TowerEnemies = towerEnemies;
        Locations = locations;
        Bosses = bosses;
        Items = items;
        allUpgradeInfos = upgradeInfos;
        foreach (Item item in items)
        {
            if(!icons.ContainsKey(item.itemName))
                icons.Add(item.itemName, item.icon);
        }
        foreach (EnemyInfo e in TowerEnemies)
        {
            e.LoadEquipment();
        }
    }

    private void OnApplicationQuit()
    {
        foreach (EnemyInfo e in TowerEnemies)
        {
            e.ResetEquipment();
        }
    }

    public static Sprite LoadImage(string name)
    {
        Sprite icon;
        icons.TryGetValue(name, out icon);
        return icon;
    }

    public static Blueprint LoadBlueprint(Blueprint b)
    {
        foreach(Item item in Items)
        {
            if(item.itemName == b.itemName)
            {
                return (Blueprint)item;
            }
        }
        return null;
    }

    public static EquipmentUpgrade FindItem(Equipment eq)
    {
        foreach(EquipmentUpgrade upgrade in allUpgradeInfos)
        {
            if(eq.itemName.Contains(upgrade.equipmentSet))
            {
                return upgrade;
            }
        }
        return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reset!");
            Data.DeleteSave();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
