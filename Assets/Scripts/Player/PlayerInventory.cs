﻿using TMPro;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    [field: SerializeField]
    private uint gold;
    public uint Gold
    {
        get { return gold; }
        set { gold = value; gold = System.Math.Clamp(gold, 0, 4000000000); }
    }
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
    [SerializeField] private TextMeshProUGUI mainGoldText;
    [SerializeField] private TextMeshProUGUI inventoryGoldText;

    void Awake()
    {
        Instance = this;
        slots = itemList.GetComponentsInChildren<InventorySlot>();
    }

    private void Start()
    {     
        RefreshUI();
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                slots[i].ClearSlot();
            else
                slots[i].FillSlot(slots[i].item, slots[i].amount);
        }

        mainGoldText.text = data.Gold.ToString();
        inventoryGoldText.text = data.Gold.ToString();
    }

    public bool AddItem(Item item, int amount)
    {
        if (IsFull()) return false;

        if (item.stackable)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.item == null) continue;

                if (slot.item.ID == item.ID && slot.amount < slot.item.maxInStack)
                {
                    int toFullStack = item.maxInStack - slot.amount;
                    if(toFullStack < amount)
                    {
                        amount -= toFullStack;
                        slot.amount += amount;
                        AddItem(item, amount);
                    }
                    else
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

    public void ChangeGoldAmount(int amount)
    {
        data.Gold += (uint)amount;
        mainGoldText.text = data.Gold.ToString();
        inventoryGoldText.text = data.Gold.ToString();
    }

    public void RemoveItem(Item _item, int amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null) continue;

            if (slot.item.ID == _item.ID)
            {
                int itemAmount = slot.amount;
                slot.amount -= amount;
                amount -= itemAmount;

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
        if (slots[slots.Length - 1] != null)
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
                int price = slot.item.GetSellPrice() * slot.amount;
                ChangeGoldAmount(price);
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

    public bool HaveEnoughGold(int neededGold)
    {
        if (data.Gold >= neededGold)
            return true;
        return false;
    }

    public bool HaveAllMaterials(Blueprint b)
    {
        foreach (ItemStack item in b.requiredItems)
        {
            if (!HaveItem(item.item, item.amount)) 
                return false;
        }
     
        return true;
    }

    public void RemoveMaterials(Blueprint b)
    {
        foreach (ItemStack item in b.requiredItems)
        {
            RemoveItem(item.item, item.amount);
        }
        RefreshUI();
    }

    public int HowMany(Item item)
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

    public bool HaveEnoughSlots(ItemStack[] items)
    {
        int neededSlots = items.Length;
        int emptySlots = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null) emptySlots++;
        }
        foreach (ItemStack itemStack in items)
        {
            if(itemStack.item.stackable)
            {
                foreach (InventorySlot slot in slots)
                {
                    if (slot.item == null) continue;

                    if (slot.item.ID == itemStack.item.ID)
                    {
                        int toFullStack = slot.item.maxInStack - slot.amount;
                        itemStack.amount -= toFullStack;
                        if (itemStack.amount <= 0)
                        {
                            neededSlots--;
                            break;                         
                        }                         
                    }
                }
            }
        }
        if (emptySlots < neededSlots)
        {
            GameManager.Instance.ShowText($"Inventory is full", Color.red);
            return false;
        }    
        return true;
    }
}
