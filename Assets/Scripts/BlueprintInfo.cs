using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlueprintInfo : MonoBehaviour
{
    [SerializeField] private InventorySlot resultItem;
    [SerializeField] private Transform itemList;
    [SerializeField] private GameObject neededItemPrefab;

    public void SetUp(Blueprint b)
    {
        resultItem.AddItem(b.resultItem);

        foreach (NeededItem needItem in b.neededItems)
        {
            GameObject item = Instantiate(neededItemPrefab, itemList);
            item.transform.Find("Icon").GetComponent<Image>().sprite = needItem.item.icon;
            TextMeshProUGUI quantity = item.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
            quantity.text = needItem.quantity.ToString();
            if (Inventory.Instance.info.CheckItem(needItem))
                quantity.color = Color.green;
            else
                quantity.color = Color.red;
        }
    }
}
