using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool inventory;

    private InventorySlot invSlot;
    private EquipmentSlot eqSlot;

    private void Start()
    {
        if(inventory)
            invSlot = GetComponentInParent<InventorySlot>();
        else
            eqSlot = GetComponentInParent<EquipmentSlot>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (inventory)
            {
                if (invSlot.item.itemType == ItemType.Equipment)
                {
                    Equipment eq = (Equipment)invSlot.item;
                    Player.Instance.Equip(eq, invSlot);
                }
            }
            else
            {
                Player.Instance.Unequip(eqSlot.item, null);
            }           
        }
    }
}
