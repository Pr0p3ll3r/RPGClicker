using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
                if (slot.item == item)
                {
                    slot.AddAmount(amount);
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
            if (slot.item == _item)
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
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == _item)
            {
                slots[i].item = null;
            }
        }
        RefreshUI();
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
        return true;
    }

    public bool HaveItem(Item item, int amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item)
            {
                amount -= slot.amount;
            }
        }

        if (amount <= 0) return true;
        else return false;
    }

    public void DisplayItemInfo(Item item, bool inventory)
    {
        selectedItemInfo.SetActive(false);
        equippedItemInfo.SetActive(false);
        if (inventory)
        {
            if (item.itemType == ItemType.Equipment)
            {
                Equipment selectedEq = (Equipment)item; 
                selectedItemInfo.GetComponent<ItemInfo>().SetUp(selectedEq, false);
                selectedItemInfo.SetActive(true);
                Equipment equippedEq = PlayerEquipment.Instance.slots[(int)selectedEq.equipmentTypeSlot].item;
                if (equippedEq != null)
                {
                    equippedItemInfo.GetComponent<ItemInfo>().SetUp(equippedEq, true);
                    equippedItemInfo.SetActive(true);
                }               
            }
            else
            {
                selectedItemInfo.GetComponent<ItemInfo>().SetUp(item, false);
                selectedItemInfo.SetActive(true);
            }
        }
        else
        {
            Equipment equippedEq = (Equipment)item;
            equippedItemInfo.GetComponent<ItemInfo>().SetUp(equippedEq, true);
            equippedItemInfo.SetActive(true);
        }           
    }

    public bool CheckGold(int neededGold)
    {
        if (data.gold >= neededGold)
        {
            data.gold -= neededGold;
            return true;
        }
        else return false;
    }

    public bool CheckResources(Blueprint b)
    {
        foreach (NeedItem item in b.needItems)
        {
            if (!HaveItem(item.item, item.amount)) 
                return false;
        }

        foreach (NeedItem item in b.needItems)
        {
            RemoveItem(item.item, item.amount);
        }

        RefreshUI();
        return true;
    }
}
