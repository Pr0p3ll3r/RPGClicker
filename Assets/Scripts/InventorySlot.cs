using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantity;

    private Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;
        quantity.text = item.quantity.ToString();
        icon.sprite = item.icon;
        icon.enabled = true;

        if (newItem.quantity == 1) quantity.gameObject.SetActive(false);
        else quantity.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        item = null;
        quantity.text = "";
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;

        Inventory.Instance.DisplayItemInfo(item, true);
    }
}

