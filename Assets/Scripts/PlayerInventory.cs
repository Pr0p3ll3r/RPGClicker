using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[System.Serializable]
public class InventoryData
{
    public int gold;
    public InventoryData()
    {
        gold = 0;
    }
}

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public InventorySlot[] slots;
    [SerializeField] private Transform itemList;
    public InventoryData data = new InventoryData();
    [SerializeField] private List<Item> startingItems = new List<Item>();
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GameObject equippedItemInfo;
    [SerializeField] private GameObject selectedItemInfo;

    void Awake()
    {
        Instance = this;
        slots = itemList.GetComponentsInChildren<InventorySlot>();
    }

    private void Start()
    {     
        if (PlayerPrefs.GetInt("NewGame", 1) == 1)
            foreach (Item item in startingItems)
            {
                AddItem(item.GetCopy(), 1);
            }

        equippedItemInfo.SetActive(false);
        selectedItemInfo.SetActive(false);
        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                slots[i].ClearSlot();
            else
                slots[i].FillSlot(slots[i].item);
        }

        goldText.text = data.gold.ToString();
    }

    public bool AddItem(Item item, int amount)
    {
        if (IsFull()) return false;

        if (item.stackable)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.item == null) continue;

                if (slot.item.ID == item.ID)
                {
                    slot.amount += amount;
                    RefreshUI();
                    return true;
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item;
                slots[i].amount = amount;
                RefreshUI();
                return true;
            }
        }
        return false;
    }

    public void AddGold(int amount)
    {
        data.gold += amount;
        goldText.text = data.gold.ToString();
    }

    public void RemoveItem(Item _item, int amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null) continue;

            if (slot.item.ID == _item.ID)
            {
                int itemAmount = slot.amount;
                amount -= itemAmount;
                slot.amount -= itemAmount;

                if (slot.amount <= 0) RemoveItem(slot.item);
                if (amount == 0) break;
            }
        }
    }

    public void RemoveItem(Item _item)
    {
        int i;
        for (i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == _item)
            {
                slots[i].item = null;
                slots[i].amount = 0;
                break;
            }
        }

        // Shifting elements
        for (; i < slots.Length - 1; ++i)
        {
            slots[i].item = slots[i + 1].item;
            slots[i].amount = slots[i + 1].amount;
        }
        if (slots[slots.Length-1] != null)
        {
            slots[slots.Length - 1].item = null;
            slots[slots.Length - 1].amount = 0;
        }

        RefreshUI();
    }

    public void SellItem(Item _item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item)
            {
                int price = slot.item.price * slot.amount;
                AddGold(price);
                GameManager.Instance.ShowText($"+{price} Gold", Color.yellow);
                RemoveItem(slot.item);
                break;
            }
        }
    }

    public bool IsFull()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null)
            {
                return false;
            }
        }
        GameManager.Instance.ShowText($"Inventory is full", Color.red);
        return true;
    }

    public bool HaveItem(Item item, int amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null) continue;

            if (slot.item.ID == item.ID)
            {
                amount -= slot.amount;
            }
        }

        if (amount <= 0) return true;
        else return false;
    }

    public void DisplayItemInfo(Item item, bool inventory)
    {
        PlayerEquipment equipment = PlayerEquipment.Instance;
        selectedItemInfo.SetActive(false);
        equippedItemInfo.SetActive(false);
        if (inventory)
        {
            if (item.itemType == ItemType.Equipment)
            {
                Equipment selectedEq = (Equipment)item;
                int index = Utils.GetRightSlot(selectedEq);
                Equipment equippedEq = null;
                if (index > -1)
                    equippedEq = equipment.slots[index].item;
                selectedItemInfo.GetComponent<ItemInfo>().SetUp(selectedEq, false, equippedEq);
                selectedItemInfo.SetActive(true);
                //compare with a first empty one or the first one            
                if (equippedEq != null)
                {
                    equippedItemInfo.GetComponent<ItemInfo>().SetUp(equippedEq, true, selectedEq);
                    equippedItemInfo.SetActive(true);
                }               
            }
            else
            {
                selectedItemInfo.GetComponent<ItemInfo>().SetUp(item, false, null);
                selectedItemInfo.SetActive(true);
            }
        }
        else
        {
            Equipment equippedEq = (Equipment)item;
            equippedItemInfo.GetComponent<ItemInfo>().SetUp(equippedEq, true, null);
            equippedItemInfo.SetActive(true);
        }           
    }

    public void CloseItemsInfo()
    {
        equippedItemInfo.SetActive(false);
        selectedItemInfo.SetActive(false);
    }

    public bool HaveEnoughGold(int neededGold)
    {
        if (data.gold >= neededGold)
            return true;
        return false;
    }

    public bool HaveAllMaterials(Blueprint b)
    {
        foreach (RequiredItem item in b.requiredItems)
        {
            if (!HaveItem(item.item, item.amount)) 
                return false;
        }
     
        return true;
    }

    public void RemoveMaterials(Blueprint b)
    {
        foreach (RequiredItem item in b.requiredItems)
        {
            RemoveItem(item.item, item.amount);
        }
        RefreshUI();
    }

    public int HaveMany(Item item)
    {
        int amount = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null) continue;

            if (slot.item.ID == item.ID)
            {
                amount += slot.amount;
            }
        }
        return amount;
    }
}
