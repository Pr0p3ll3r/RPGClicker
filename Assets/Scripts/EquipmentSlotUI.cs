using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Sprite clearIcon;

    private Equipment equipment;

    public void AddEquipment(Equipment newEquipment)
    {
        equipment = newEquipment;

        icon.sprite = equipment.icon;
    }

    public void ClearSlot()
    {
        equipment = null;

        icon.sprite = clearIcon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (equipment == null) return;

        Inventory.Instance.DisplayItemInfo(equipment, false);
    }
}
