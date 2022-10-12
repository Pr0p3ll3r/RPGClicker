using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInfo : MonoBehaviour
{
    [SerializeField] private InventorySlot resultItem;
    [SerializeField] private Transform itemList;
    [SerializeField] private GameObject neededItemPrefab;
    [SerializeField] private TextMeshProUGUI chance;

    public void SetUp(Equipment eq)
    {
        EquipmentUpgrade upgradeItem = Library.FindItem(eq);

        resultItem.AddItem(eq);

        int currentLevel = eq.level;

        for (int i = 0; i < upgradeItem.upgradeLevels[currentLevel].neededItems.Length; i++)
        {
            NeededItem needItem = upgradeItem.upgradeLevels[currentLevel].neededItems[i];
            GameObject item = Instantiate(neededItemPrefab, itemList);
            item.transform.Find("Icon").GetComponent<Image>().sprite = needItem.item.icon;
            TextMeshProUGUI quantity = item.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
            quantity.text = needItem.quantity.ToString();
            chance.text = $"{100 - eq.level * 10}%";
            if (Inventory.Instance.info.CheckItem(needItem))
                quantity.color = Color.green;
            else
                quantity.color = Color.red;
        }
    }
}
