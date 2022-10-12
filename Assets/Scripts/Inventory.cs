using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public InventoryInfo info;

    [SerializeField] private Transform itemsList;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private GameObject itemInfo;
    [SerializeField] private ItemInfo selectedItemInfo;
    [SerializeField] private ItemInfo equippedItemInfo;

    private int maxItems;
    private InventorySlot[] slots;

    void Awake()
    {
        Instance = this; 
        info = ScriptableObject.CreateInstance<InventoryInfo>();
        slots = itemsList.GetComponentsInChildren<InventorySlot>();
        maxItems = slots.Length;    
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < info.items.Count; i++)
        {
            slots[i].AddItem(info.items[i]);
        }

        for (int i = info.items.Count; i < slots.Length; i++)
        {
            slots[i].ClearSlot();
        }

        gold.text = info.gold.ToString();
    }

    public void AddItem(Item item)
    {
        if (!CheckSpace()) return;

        info.Add(item);

        //if (info.items.Count == (maxItems - 1))
        //{
        //    GameManager.Instance.ShowText("One empty slot in the inventory", new Color32(255, 65, 52, 255));
        //}

        Player.Instance.RefreshPlayer();
        UpdateUI();
    }

    public bool CheckSpace()
    {
        if (info.items.Count == maxItems)
        {
            GameManager.Instance.ShowText("INVENTORY FULL", new Color32(255, 65, 52, 255));
            return false;
        }
        else
            return true;
    }

    public void RemoveItem(Item item)
    {
        info.Remove(item);
        Player.Instance.RefreshPlayer();
        UpdateUI();
    }

    public void RemoveItem(Item item, int quantity)
    {
        info.Remove(item, quantity);
        Player.Instance.RefreshPlayer();
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        info.gold += amount;
        UpdateUI();
    }

    public bool CheckGold(int amount)
    {
        if(info.gold >= amount)
        {
            info.gold -= amount;
            UpdateUI();
            return true;
        }
        return false;              
    }

    public bool CheckResources(Blueprint b)
    {
        foreach(NeededItem item in b.neededItems)
        {
            if (!info.CheckItem(item)) return false;
        }

        foreach (NeededItem item in b.neededItems)
        {
            info.Remove(item.item, item.quantity);
        }

        UpdateUI();

        return true;
    }

    public bool CheckResources(EquipmentUpgrade upgrade, int level)
    {
        for (int i = 0; i < upgrade.upgradeLevels[level].neededItems.Length; i++)
        {
            NeededItem item = upgrade.upgradeLevels[level].neededItems[i];
            if (!info.CheckItem(item)) return false;
        }

        for (int i = 0; i < upgrade.upgradeLevels[level].neededItems.Length; i++)
        {
            NeededItem item = upgrade.upgradeLevels[level].neededItems[i];
            info.Remove(item.item, item.quantity);
        }

        UpdateUI();

        return true;
    }

    public void DisplayItemInfo(Item item, bool inventory)
    {
        itemInfo.SetActive(true);

        if (inventory)
        {
            if (item.GetType() == typeof(Equipment))
            {
                Equipment eq = (Equipment)item;
                int eqSlot = (int)eq.equipSlot;
                selectedItemInfo.SetUp((Equipment)item);
                equippedItemInfo.SetUp(Player.Instance.info.equipment[eqSlot]);
            }
            else
            {
                selectedItemInfo.SetUp(item);
                equippedItemInfo.SetUp(null);
            }
        }
        else
        {
            Equipment eq = (Equipment)item;
            int eqSlot = (int)eq.equipSlot;
            selectedItemInfo.SetUp(null);
            equippedItemInfo.SetUp(Player.Instance.info.equipment[eqSlot]);
        }
    }
}
