using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance { get; private set; }

    public EquipmentSlot[] slots;
    [SerializeField] private Transform equipmentParent;
    [SerializeField] private PlayerUI hud;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        slots = equipmentParent.GetComponentsInChildren<EquipmentSlot>();
        RefreshUI();
    }

    public void EquipItem(Equipment item, out Equipment previousItem)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].equipmentType == item.equipmentTypeSlot)
            {
                previousItem = slots[i].item;
                slots[i].item = item;
                Debug.Log("Equipped Item: " + item.itemName);
                return;
            }
        }
        previousItem = null;
    }

    public void UnequipItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                slots[i].item = null;
                Debug.Log("Unequipped Item: " + item.itemName);
            }
        }
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
    }
}
