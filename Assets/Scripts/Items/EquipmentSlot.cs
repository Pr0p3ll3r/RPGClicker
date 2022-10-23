﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite placeholderIcon;

    public Equipment item;
    public EquipmentTypeSlot equipmentType;

    public bool CanPlaceInSlot(Equipment _item)
    {
        if (_item.equipmentTypeSlot == equipmentType)
            return true;
        return false;
    }

    public void FillSlot(Equipment _item)
    {
        item = _item;
        icon.enabled = true;   
        icon.raycastTarget = true;
        icon.sprite = Database.Instance.itemDatabase.GetItemById(_item.ID).icon;                 
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = placeholderIcon;
        icon.raycastTarget = false;
    }
}
