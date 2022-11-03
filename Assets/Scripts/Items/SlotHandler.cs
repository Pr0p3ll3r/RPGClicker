using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool inventory;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(inventory)
            PlayerInventory.Instance.DisplayItemInfo(GetComponentInParent<InventorySlot>().item, inventory); 
        else
            PlayerInventory.Instance.DisplayItemInfo(GetComponentInParent<EquipmentSlot>().item, inventory);
    }
}
